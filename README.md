<h1> Confront </h1>

<h2>環境</h2>
使用エンジン : Unity </br>
バージョン : 2021.3.6 </br>
制作者名 : <br/>
プログラマー 丸岡晴海 </br>

<h2>このプロジェクトを立ち上げたきっかけ</h2>
・年度末の審査会のテーマが3Dだったので新しいプロジェクトを立ちあげた <br/>
・年度末の審査会では優勝を目指す <br/>

<h2> このプロジェクトのテーマ / 目標 </h2>

<h3> ゲームとしての目標 </h3>
・かっこよく！ <br/>
・カジュアルで！ <br/>
・スピード感がある！ <br/>
<br/>
　を感じられるゲームを作成する！<br/>
<br/>

<h3> プログラマとしての目標 </h3>
<ul>
<li><p>これまでの経験を活かす。
    <ul>
    <li><p> 前回のプロジェクト(2Dアクション : Overcome) で作成した「アイテム」や「装備」などのパワーアップ機能を今回も使用する。 </p></li>
    </ul>
<li><p>昨日の自分より一歩前へ。
    <ul>
    <li><p> Overcomeでは設計面の弱さが作り直しを招くことになり2度手間、3度手間を生んでいる。その面を克服する。</p></li>
    <li><p> 又、前回作成したものを今回はより多機能化し、より軽い処理を目指す。</p></li>
    </ul>
<li><p>UnityRoomに投稿、あるいはSteamにリリースを目標に作る。
    <ul>
    <li><p> 挑戦する。</p></li>
    </ul>
</ul>

<h3> コーディング規約 </h3>
・イベントは On〇〇 <br/>
・bool値は フィールドの場合_is〇〇 プロパティの場合 Is〇〇と記述すること <br/>
・Debugクラスを使用してConsoleに何か表示する際はカテゴリーによって色を分ける。<br/>
・summaryタグで何事にも目的と意図を明確に記す。<br/>
・「人に見せるもの」という意識を持ってコードを書く。読みやすいコードを書く<br/>
・Dry原則に従って出来るだけ同じコードを書かないように意識する。<br/>
・KISS原則に従って「単純性」「簡潔性」を重視して記述すること。<br/>
・他にも決まり次第ここに追記すること。
・誰でも読めるコードを目指そう

<h3> 他の決め事 </h3>

<h3> 仕様書的な何か </h3>
状態遷移図
https://viewer.diagrams.net/?tags=%7B%7D&highlight=0000ff&edit=_blank&layers=1&nav=1&title=%E5%90%8D%E7%A7%B0%E6%9C%AA%E8%A8%AD%E5%AE%9A%E3%83%95%E3%82%A1%E3%82%A4%E3%83%AB.drawio#R1Vxbd6M2EP41fnQOIED2YzbJXtrmdNv0snnqoUa12WDLB%2BPY7q9fsIWBISAZo0vyEjQegaT55irBCN0t95%2BSYL14pCGJR44V7kfofuQ4toUm2b%2BccmAU7FonyjyJQkYrCU%2FR%2F6ToyqjbKCSbGmNKaZxG6zpxRlcrMktrtCBJ6K7O9h%2BN609dB3PSIDzNgrhJ%2FTsK08WJOnFwSf9MovmieLLtT0%2B%2FLIOCmc1kswhCuquQ0MMI3SWUpqer5f6OxPnqFety6vex5dfzwBKySkU6fH6Z%2B79%2B%2Bf7xhSa%2Fx88%2F%2F%2FXptz%2F%2FGWOXDS49FDMmYbYArEmTdEHndBXEDyX1Q0K3q5Dkt7WyVsnzC6XrjGhnxO8kTQ9MmsE2pRlpkS5j9ivZR%2Bm3yvVzfqsb7LHm%2FZ7d%2Btg4FI1Vmhy%2BVRvVbnm77HdsFR1PM8yn1bpyjLSh22RGOparQGCQzEnataxn%2BWaaQeiSZOPJ%2BiUkDtLotT6OgCF0fuYrhZhdMDleIlPPHJn2E6mBEvWvlOix622SBIcKw5pGq3RTufPXnJAxFPZzwmwHs54%2B0HAOu40tAKbTAEponWfSH21szq9BvGWrcB8sc6MKMVhH2G4RpeRpHRyFs8ucRx1N7KYkScm%2BW8ZNmbAOTn0xEGvuSituFyyLigVnazy8Wvp2Y0kUqqVVU0unl16q00pHkZ19W48cqw4dF0DiNCzWqUTFpdrdeIzndao35EcKtNtpaPcfQfyiX7d9sHSubu32DNJuUeUu%2FexzcT816o0E1bsFBGrCqAIqJkhU2F7fWBaqx1IOnnIEe2x9JUmUrRtJ9ElbkjEHccA5sxzamsN4g2fNAb%2BLu9mhxauxyzH%2B2CANeAfJoSjOjUgl0KQbno1cos4vCXCOTsC9u2qEKOCQHMvqAZNk%2B9ObaeUPYymG1oOGloNkyK8EyUw0lbj5MQqDKGngW3fkjPRHzgYpvXCYhWuhs8LM2BXU%2BWtD516qeS7ui9WrIPsYdfPbdie%2FHE32DYKneBpgPD4HcUkX4xPG3N2eA7Lb3fBEXhe7HHS6DT9zm6bBTH%2BFxoPxpm43g6Y69bhngQbrqtB4itzMdZGDQSLtV1FXudMlKlKte5f%2BRKtIy6zvufKTud5WVKZIq5o2fNRP2%2BVau4eCmZCjPxMyqN4mas%2FskR4H5StyUP2KbSCxsT0FJQcP6zWdTtV0WqKm09EV4YgCSK87NOh4Vi%2BLoLYgKipTZwij0NB6F27RTOXsNMHn2Jydpsa4VBRAfXMOoXXCdmj0DV9lf%2FsMmHRQud3FDsjPKfW1zEIuBDU7xHeWS4gaT0%2BrQ9Sa8vfbIQQO0THQIV6bH%2FYzOd1bzFx%2BpCKqxlrPddmXWxDTE7JBYq%2BL94zB0cniKGWruwL8nqXCX41gneSRvhpwjrolMtBXJ3H11kl6Zbp1J6BOLbGgWmqpkzhTAC3O%2Fi8sq6jYYENasNYSO%2FBCh8v9hgaoXRtAtuy%2BAixNgPU5Df%2F6PAl4hiknTYKOR8UrOa5BVaOeQbLCGFkUtoPEyE2ANE6zScrvwXMwJ9aGw1KCW3NqRpwt%2BqHhN7w1BLmSiyXB6rITX5Ddxd38bdOQC0M9daOWEwLcckGvar0G%2B%2BlKAbrf4mB5QH8Dak7tRp6c%2BMHDANGcw8ud7JJKHiOYh34JY%2FPyUO0nyiaNdWrajVV4m3%2BEImvN4mCziWb1Zamv4bstHYuagJYdkooEvTcEWNCujbQQx1K0aDi3Dnl%2Bk4tjcgbb6NeTkl53YF4XNJHSAkizZAHi6LGktw9x96cfuPzF2aM2%2Fm52SfVQg3Bu%2BgcThGEuJ4tF8E0OSbvU%2BMKjD5B%2FzKswXsmPXAV6gQ3SC%2BG3JM13AFreSJmA9IW3Bwr5x7wEGvofFQk0amYQxduPfpyJ4MO%2F%2BdU8vzL0bRWZe1xZs%2Fzo22nJy2%2FnoYcf <br/>
https://docs.google.com/spreadsheets/d/1l35ytyqEfQ2Ws91MpLVoRpL0s4FeUuzkA5XVEoQggyA/edit#gid=0 <br/>
