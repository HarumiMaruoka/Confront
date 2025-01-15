using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public static class HemisphereRaycastUtility
{
    /// <summary>
    /// 半球状に放射状に Raycast を行い、最も近く衝突したオブジェクトの法線を返す。
    /// ヒットしなかった場合は Vector3.zero を返す。
    /// </summary>
    /// <param name="origin">レイを発射する中心位置</param>
    /// <param name="up">半球の“上”となる方向</param>
    /// <param name="radius">レイの最大長さ</param>
    /// <param name="rayCount">レイを発射する本数</param>
    /// <param name="layerMask">Raycast が当たるレイヤーマスク</param>
    /// <returns>最も近く衝突した法線ベクトル（衝突しなかった場合は Vector3.zero）</returns>
    public static Vector3 GetClosestHitNormalInHemisphere(
        Vector3 origin,
        Vector3 up,
        float radius,
        int rayCount,
        int layerMask = Physics.DefaultRaycastLayers,
        bool ignoreBackfaceHits = false
    )
    {
        if (rayCount <= 0)
        {
            Debug.LogWarning("rayCount は 1 以上にしてください。");
            return Vector3.zero;
        }

        // RaycastCommand・RaycastHit を格納する NativeArray を作成
        NativeArray<RaycastCommand> commands = new NativeArray<RaycastCommand>(rayCount, Allocator.TempJob);
        NativeArray<RaycastHit> results = new NativeArray<RaycastHit>(rayCount, Allocator.TempJob);

        // 半球をサンプリングし、RaycastCommand をセット
        // ここでは簡易的に Random.onUnitSphere を使い、up と逆向きなら反転して半球方向にそろえています。
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 dir = Random.onUnitSphere;
            // up ベクトルと同じ半球にそろえる
            if (Vector3.Dot(dir, up) < 0f)
            {
                dir = -dir;
            }

            // RaycastCommand の作成
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
            commands[i] = new RaycastCommand(origin, dir, radius, layerMask);
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
        }

        // バッチ Raycast をスケジューリング
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 64);
        // メインスレッドで完了を待機
        handle.Complete();

        // 最も近い衝突点を探し、その法線を取得
        float minDist = float.MaxValue;
        Vector3 closestNormal = Vector3.zero;

        for (int i = 0; i < rayCount; i++)
        {
            RaycastHit hit = results[i];
            var isBackfaceHit = ignoreBackfaceHits || Vector3.Dot(hit.normal, up) < -0.08f; // ノーマルが上方向と逆向きの場合は無視
            // RaycastHit.distance が 0 の場合は未ヒット
            if (hit.distance > 0f && hit.distance < minDist && isBackfaceHit)
            {
                minDist = hit.distance;
                closestNormal = hit.normal;
            }
        }

        // NativeArray を破棄
        commands.Dispose();
        results.Dispose();

        return closestNormal;
    }

    /// <summary>
    /// 半球方向（up を軸）に φ, θ で均等にレイを飛ばし、最も近いヒットの法線を返す。
    /// </summary>
    /// <param name="origin">レイの発射元</param>
    /// <param name="up">半球の軸となる上方向</param>
    /// <param name="radius">Raycast の最大距離</param>
    /// <param name="phiSegments">円周方向の分割数 (φ の分割数)</param>
    /// <param name="thetaSegments">0～90度までの分割数 (θ の分割数)</param>
    /// <param name="layerMask">Raycast の対象レイヤーマスク</param>
    /// <returns>最も近いヒットの法線（ヒットなしの場合は Vector3.zero）</returns>
    public static Vector3 GetClosestHitNormalInHemisphereUniform(
        Vector3 origin,
        Vector3 up,
        float radius,
        int phiSegments,
        int thetaSegments,
        int layerMask = Physics.DefaultRaycastLayers
    )
    {
        // 不正なパラメータチェック
        if (phiSegments < 1 || thetaSegments < 1)
        {
            Debug.LogWarning("phiSegments, thetaSegments は 1 以上を指定してください。");
            return Vector3.zero;
        }

        // 分割数に応じて、作るコマンド数を計算
        int totalRays = phiSegments * thetaSegments;

        // NativeArray を確保
        NativeArray<RaycastCommand> commands = new NativeArray<RaycastCommand>(totalRays, Allocator.TempJob);
        NativeArray<RaycastHit> results = new NativeArray<RaycastHit>(totalRays, Allocator.TempJob);

        // up を Vector3.up に合わせるための回転を作成
        Quaternion toUpRotation = Quaternion.FromToRotation(Vector3.up, up.normalized);

        // レイを均等にサンプリング
        // θ: 0 -> π/2 (0 -> 90°)
        // φ: 0 -> 2π (0 -> 360°)
        int index = 0;
        for (int i = 0; i < phiSegments; i++)
        {
            // φ は円周方向
            float phi = 2f * Mathf.PI * i / phiSegments;
            for (int j = 0; j < thetaSegments; j++)
            {
                // θ は 0 ~ π/2 (上から横まで) なので半球になる
                // j が 0 のとき θ=0 (真上)、j が thetaSegments-1 のとき θ=π/2 (水平)
                float theta = (Mathf.PI * 0.5f) * j / (thetaSegments - 1);

                // ローカル座標系 (up = +Y) の場合の方向ベクトル
                // x = sinθ cosφ
                // y = cosθ
                // z = sinθ sinφ
                float sinTheta = Mathf.Sin(theta);
                float cosTheta = Mathf.Cos(theta);
                float cosPhi = Mathf.Cos(phi);
                float sinPhi = Mathf.Sin(phi);

                Vector3 localDir = new Vector3(
                    sinTheta * cosPhi,
                    cosTheta,
                    sinTheta * sinPhi
                );

                // up を軸とするように回転
                Vector3 worldDir = toUpRotation * localDir;

                // RaycastCommand の作成 (発射元, 方向, 距離, レイヤーマスク)
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                commands[index] = new RaycastCommand(origin, worldDir, radius, layerMask);
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
                index++;
            }
        }

        // バッチ Raycast をスケジューリング (minCommandsPerJob は適宜調整)
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 64);
        // 処理完了を待つ
        handle.Complete();

        // もっとも近いヒットを探す
        float minDist = float.MaxValue;
        Vector3 closestNormal = Vector3.zero;

        for (int i = 0; i < totalRays; i++)
        {
            RaycastHit hit = results[i];
            // distance が 0 の場合、ヒットなしとみなす
            if (hit.distance > 0f && hit.distance < minDist && Vector3.Dot(hit.normal, up) < -0.08f)
            {
                minDist = hit.distance;
                closestNormal = hit.normal;
            }
        }

        // NativeArray を破棄
        commands.Dispose();
        results.Dispose();

        return closestNormal;
    }
}
