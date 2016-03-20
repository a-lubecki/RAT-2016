<map version="freeplane 1.3.0">
<!--To view this file, download free mind mapping software Freeplane from http://freeplane.sourceforge.net -->
<node TEXT="Nouvelle Carte" ID="ID_1065600223" CREATED="1432401341672" MODIFIED="1432471277481"><hook NAME="MapStyle" zoom="0.824">
    <properties show_icon_for_attributes="true" show_note_icons="true"/>

<map_styles>
<stylenode LOCALIZED_TEXT="styles.root_node">
<stylenode LOCALIZED_TEXT="styles.predefined" POSITION="right">
<stylenode LOCALIZED_TEXT="default" MAX_WIDTH="600" COLOR="#000000" STYLE="as_parent">
<font NAME="SansSerif" SIZE="10" BOLD="false" ITALIC="false"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.details"/>
<stylenode LOCALIZED_TEXT="defaultstyle.note"/>
<stylenode LOCALIZED_TEXT="defaultstyle.floating">
<edge STYLE="hide_edge"/>
<cloud COLOR="#f0f0f0" SHAPE="ROUND_RECT"/>
</stylenode>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.user-defined" POSITION="right">
<stylenode LOCALIZED_TEXT="styles.topic" COLOR="#18898b" STYLE="fork">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.subtopic" COLOR="#cc3300" STYLE="fork">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.subsubtopic" COLOR="#669900">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.important">
<icon BUILTIN="yes"/>
</stylenode>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.AutomaticLayout" POSITION="right">
<stylenode LOCALIZED_TEXT="AutomaticLayout.level.root" COLOR="#000000">
<font SIZE="18"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,1" COLOR="#0033ff">
<font SIZE="16"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,2" COLOR="#00b439">
<font SIZE="14"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,3" COLOR="#990000">
<font SIZE="12"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,4" COLOR="#111111">
<font SIZE="10"/>
</stylenode>
</stylenode>
</stylenode>
</map_styles>
</hook>
<node TEXT="LINK" POSITION="left" ID="ID_1974256138" CREATED="1432404680114" MODIFIED="1433670501251"><richcontent TYPE="NOTE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      TEST map pac man
    </p>
  </body>
</html>
</richcontent>
<node TEXT="pos" ID="ID_50036863" CREATED="1432405149698" MODIFIED="1433598624484">
<node TEXT="3" OBJECT="java.lang.Long|3" ID="ID_1701004107" CREATED="1432405166477" MODIFIED="1434890931342"/>
<node TEXT="31" OBJECT="java.lang.Long|31" ID="ID_1663653299" CREATED="1432407013415" MODIFIED="1434890932653"/>
</node>
<node TEXT="nextPos" ID="ID_1815333633" CREATED="1432404689629" MODIFIED="1433598638880">
<node TEXT="15" ID="ID_90113612" CREATED="1432401687046" MODIFIED="1433695307126"/>
<node TEXT="24" ID="ID_1975402315" CREATED="1432406999705" MODIFIED="1433670419618"/>
</node>
<node TEXT="nextDirection" ID="ID_150510254" CREATED="1432407783124" MODIFIED="1432470836636">
<node TEXT="LEFT" ID="ID_1264185617" CREATED="1432407802338" MODIFIED="1433609520245"/>
</node>
<node TEXT="nextMap" ID="ID_1803220401" CREATED="1433670336919" MODIFIED="1433670350266">
<node TEXT="&quot;Part0.PacMan&quot;" ID="ID_1628554072" CREATED="1433670351637" MODIFIED="1433670508928"/>
</node>
</node>
<node TEXT="SPAWN" POSITION="left" ID="ID_937269768" CREATED="1432470553574" MODIFIED="1435520478271">
<node TEXT="pos" ID="ID_1639010071" CREATED="1432470565135" MODIFIED="1432470820743">
<node TEXT="3" ID="ID_72891943" CREATED="1432470628181" MODIFIED="1433798140030"/>
<node TEXT="25" ID="ID_933809086" CREATED="1432470631719" MODIFIED="1433797947252"/>
</node>
<node TEXT="direction" ID="ID_1073691643" CREATED="1432408015429" MODIFIED="1432470824298">
<node TEXT="RIGHT" ID="ID_125402079" CREATED="1432408034051" MODIFIED="1433794730973"/>
</node>
</node>
<node TEXT="SPAWN_test" POSITION="left" ID="ID_800210242" CREATED="1432470553574" MODIFIED="1435520473003"><richcontent TYPE="NOTE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      TEST
    </p>
  </body>
</html>
</richcontent>
<node TEXT="pos" ID="ID_344948382" CREATED="1432470565135" MODIFIED="1432470820743">
<node TEXT="32" OBJECT="java.lang.Long|32" ID="ID_438516756" CREATED="1432470628181" MODIFIED="1435430855455"/>
<node TEXT="20" OBJECT="java.lang.Long|20" ID="ID_848312202" CREATED="1432470631719" MODIFIED="1435430857595"/>
</node>
<node TEXT="direction" ID="ID_1885430096" CREATED="1432408015429" MODIFIED="1432470824298">
<node TEXT="UP" ID="ID_577044597" CREATED="1432408034051" MODIFIED="1435430861783"/>
</node>
</node>
<node TEXT="HUB" POSITION="left" ID="ID_1641549906" CREATED="1432404854264" MODIFIED="1432404875028">
<node TEXT="pos" ID="ID_836864270" CREATED="1432404877947" MODIFIED="1432470839264">
<node TEXT="32" OBJECT="java.lang.Long|32" ID="ID_236808048" CREATED="1432404889628" MODIFIED="1435422328727"/>
<node TEXT="14" OBJECT="java.lang.Long|14" ID="ID_782007176" CREATED="1432407018573" MODIFIED="1435422331065"/>
</node>
<node TEXT="spawnDirection" ID="ID_1836946458" CREATED="1432406847591" MODIFIED="1435422254152">
<node TEXT="UP" ID="ID_1709074855" CREATED="1432406874820" MODIFIED="1432406878666"/>
</node>
<node TEXT="listener" ID="ID_536873313" CREATED="1435485595270" MODIFIED="1435520397991">
<node TEXT="in" ID="ID_997111531" CREATED="1435485648168" MODIFIED="1435485669878">
<node TEXT="&quot;onHubActivated&quot;" ID="ID_982571489" CREATED="1435485670929" MODIFIED="1435492555909"/>
</node>
<node TEXT="out" ID="ID_546255226" CREATED="1435485601977" MODIFIED="1435485705469">
<node TEXT="&quot;onFirstHubActivated&quot;" ID="ID_778509960" CREATED="1435485613193" MODIFIED="1435492563509"/>
</node>
</node>
</node>
<node TEXT="LINK" POSITION="left" ID="ID_265941405" CREATED="1432404680114" MODIFIED="1432404686238">
<node TEXT="pos" ID="ID_1015970352" CREATED="1432404689629" MODIFIED="1432470827285">
<node TEXT="32" ID="ID_627192110" CREATED="1432401687046" MODIFIED="1433794672508"/>
<node TEXT="0" ID="ID_701244658" CREATED="1432406999705" MODIFIED="1433794675699"/>
</node>
<node TEXT="width" ID="ID_868136512" CREATED="1443012028279" MODIFIED="1443012032088">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_673825728" CREATED="1443012032761" MODIFIED="1443012033887"/>
</node>
<node TEXT="nextPos" ID="ID_234463614" CREATED="1432405149698" MODIFIED="1432470833284">
<node TEXT="6" OBJECT="java.lang.Long|6" ID="ID_1301681918" CREATED="1432405166477" MODIFIED="1443012041074"/>
<node TEXT="78" OBJECT="java.lang.Long|78" ID="ID_609207647" CREATED="1432407013415" MODIFIED="1438982033929"/>
</node>
<node TEXT="nextDirection" ID="ID_919798014" CREATED="1432407783124" MODIFIED="1432470836636">
<node TEXT="UP" ID="ID_258808298" CREATED="1432407802338" MODIFIED="1433794745428"/>
</node>
<node TEXT="nextMap" ID="ID_1922805032" CREATED="1433696934563" MODIFIED="1433696938325">
<node TEXT="&quot;Part1.Laboratory2&quot;" ID="ID_635775449" CREATED="1433696949408" MODIFIED="1433696953277"/>
</node>
</node>
<node TEXT="LINK" POSITION="left" ID="ID_443311750" CREATED="1432404680114" MODIFIED="1432404686238">
<node TEXT="pos" ID="ID_755772872" CREATED="1432405149698" MODIFIED="1433598624484">
<node TEXT="48" ID="ID_1946459183" CREATED="1432405166477" MODIFIED="1433794982680"/>
<node TEXT="31" ID="ID_531706165" CREATED="1432407013415" MODIFIED="1433794984461"/>
</node>
<node TEXT="nextPos" ID="ID_1685841866" CREATED="1432404689629" MODIFIED="1433598638880">
<node TEXT="4" ID="ID_972221192" CREATED="1432401687046" MODIFIED="1433794947905"/>
<node TEXT="4" ID="ID_1234301909" CREATED="1432406999705" MODIFIED="1433794949536"/>
</node>
<node TEXT="nextDirection" ID="ID_795969423" CREATED="1432407783124" MODIFIED="1432470836636">
<node TEXT="RIGHT" ID="ID_1347288065" CREATED="1432407802338" MODIFIED="1433794941613"/>
</node>
<node TEXT="nextMap" ID="ID_570205591" CREATED="1433696934563" MODIFIED="1433696938325">
<node TEXT="&quot;Part1.Laboratory2&quot;" ID="ID_834241311" CREATED="1433696949408" MODIFIED="1433696953277"/>
</node>
</node>
<node TEXT="LINK" POSITION="left" ID="ID_1879492857" CREATED="1432404680114" MODIFIED="1435418331750"><richcontent TYPE="NOTE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      TEST
    </p>
  </body>
</html>
</richcontent>
<node TEXT="pos" ID="ID_523039525" CREATED="1432405149698" MODIFIED="1433598624484">
<node TEXT="48" ID="ID_1349265714" CREATED="1432405166477" MODIFIED="1433794796884"/>
<node TEXT="24" ID="ID_1457081924" CREATED="1432407013415" MODIFIED="1433794800628"/>
</node>
<node TEXT="nextPos" ID="ID_1137399399" CREATED="1432404689629" MODIFIED="1433598638880">
<node TEXT="2" ID="ID_1074412426" CREATED="1432401687046" MODIFIED="1433794809095"/>
<node TEXT="38" ID="ID_150545212" CREATED="1432406999705" MODIFIED="1433794812850"/>
</node>
<node TEXT="nextDirection" ID="ID_1419982474" CREATED="1432407783124" MODIFIED="1432470836636">
<node TEXT="RIGHT" ID="ID_337728897" CREATED="1432407802338" MODIFIED="1433794818499"/>
</node>
</node>
<node TEXT="LINK" POSITION="left" ID="ID_323924710" CREATED="1432404680114" MODIFIED="1435418340479"><richcontent TYPE="NOTE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      TEST
    </p>
  </body>
</html>
</richcontent>
<node TEXT="pos" ID="ID_1256457220" CREATED="1432405149698" MODIFIED="1433598624484">
<node TEXT="0" ID="ID_220903903" CREATED="1432405166477" MODIFIED="1433794837071"/>
<node TEXT="38" ID="ID_1306153111" CREATED="1432407013415" MODIFIED="1433794840033"/>
</node>
<node TEXT="nextPos" ID="ID_1701999921" CREATED="1432404689629" MODIFIED="1433598638880">
<node TEXT="46" ID="ID_1798648235" CREATED="1432401687046" MODIFIED="1433794850900"/>
<node TEXT="24" ID="ID_712777350" CREATED="1432406999705" MODIFIED="1433794912740"/>
</node>
<node TEXT="nextDirection" ID="ID_378108439" CREATED="1432407783124" MODIFIED="1432470836636">
<node TEXT="LEFT" ID="ID_529017765" CREATED="1432407802338" MODIFIED="1433794858236"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_354670920" CREATED="1433962594806" MODIFIED="1433962599016">
<node TEXT="id" ID="ID_1499832932" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door1&quot;" ID="ID_1253703059" CREATED="1437598314952" MODIFIED="1437598323025"/>
</node>
<node TEXT="pos" ID="ID_757999555" CREATED="1433962602709" MODIFIED="1433962606723">
<node TEXT="5" ID="ID_715439631" CREATED="1433962635338" MODIFIED="1433963071549"/>
<node TEXT="29" OBJECT="java.lang.Long|29" ID="ID_1668856765" CREATED="1433962669975" MODIFIED="1434887287926"/>
</node>
<node TEXT="orientation" ID="ID_1112217260" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_83578331" CREATED="1433964260806" MODIFIED="1434316102874"/>
</node>
<node TEXT="spacing" ID="ID_1356482261" CREATED="1434315855134" MODIFIED="1434315977611">
<node TEXT="1" OBJECT="java.lang.Long|1" ID="ID_1184001597" CREATED="1434315870989" MODIFIED="1434316095861"/>
</node>
<node TEXT="status" ID="ID_535834507" CREATED="1433963019992" MODIFIED="1433963023836">
<node TEXT="OPENED" ID="ID_1627429777" CREATED="1433963025262" MODIFIED="1433963036572"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_893014692" CREATED="1433962594806" MODIFIED="1433962599016">
<node TEXT="id" ID="ID_1779727861" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door2&quot;" ID="ID_557347756" CREATED="1437598314952" MODIFIED="1437598340774"/>
</node>
<node TEXT="pos" ID="ID_1565937009" CREATED="1433962602709" MODIFIED="1433962606723">
<node TEXT="17" ID="ID_1793421485" CREATED="1433962635338" MODIFIED="1433964112355"/>
<node TEXT="29" OBJECT="java.lang.Long|29" ID="ID_1510297890" CREATED="1433962669975" MODIFIED="1434887295558"/>
</node>
<node TEXT="orientation" ID="ID_484395243" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_380820781" CREATED="1433964260806" MODIFIED="1434316102874"/>
</node>
<node TEXT="spacing" ID="ID_210780253" CREATED="1434315855134" MODIFIED="1434315977611">
<node TEXT="1" OBJECT="java.lang.Long|1" ID="ID_913480959" CREATED="1434315870989" MODIFIED="1434316095861"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_127077574" CREATED="1433962594806" MODIFIED="1433962599016">
<node TEXT="id" ID="ID_1802312803" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door3&quot;" ID="ID_1881107054" CREATED="1437598314952" MODIFIED="1437598349794"/>
</node>
<node TEXT="pos" ID="ID_1165175501" CREATED="1433962602709" MODIFIED="1433962606723">
<node TEXT="8" ID="ID_1983283705" CREATED="1433962635338" MODIFIED="1433962669360"/>
<node TEXT="33" OBJECT="java.lang.Long|33" ID="ID_905828611" CREATED="1433962669975" MODIFIED="1434887302990"/>
</node>
<node TEXT="orientation" ID="ID_892507404" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_517250604" CREATED="1433964260806" MODIFIED="1434316199180"/>
</node>
<node TEXT="spacing" ID="ID_934731232" CREATED="1434315855134" MODIFIED="1434315977611">
<node TEXT="1" OBJECT="java.lang.Long|1" ID="ID_1031146197" CREATED="1434315870989" MODIFIED="1434316095861"/>
</node>
<node TEXT="unlockSide" ID="ID_1997077151" CREATED="1433964010025" MODIFIED="1434318083287">
<node TEXT="DOWN" ID="ID_539322" CREATED="1433962775922" MODIFIED="1434318513665"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_124641323" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_1715850904" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door4&quot;" ID="ID_975237744" CREATED="1437598314952" MODIFIED="1437598365703"/>
</node>
<node TEXT="pos" ID="ID_670007942" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="1" ID="ID_1405486770" CREATED="1433962916237" MODIFIED="1433962917699"/>
<node TEXT="38" ID="ID_833358763" CREATED="1433962906568" MODIFIED="1433962909368"/>
</node>
<node TEXT="orientation" ID="ID_1681489347" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="SIDE" ID="ID_293471171" CREATED="1433964260806" MODIFIED="1434316234833"/>
</node>
<node TEXT="spacing" ID="ID_1196696122" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_1019812037" CREATED="1434316247486" MODIFIED="1434316248449"/>
</node>
<node TEXT="requireItem" ID="ID_432981910" CREATED="1433963184440" MODIFIED="1456682633212">
<node TEXT="S_KEY_LABORATORY_1" ID="ID_1027262362" CREATED="1433963416765" MODIFIED="1457137398890"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_1754437409" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_623695447" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door5&quot;" ID="ID_521746779" CREATED="1437598314952" MODIFIED="1437598379655"/>
</node>
<node TEXT="pos" ID="ID_468348566" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="32" OBJECT="java.lang.Long|32" ID="ID_1859483296" CREATED="1433962916237" MODIFIED="1435418034534"/>
<node TEXT="45" OBJECT="java.lang.Long|45" ID="ID_1869775425" CREATED="1433962906568" MODIFIED="1435418036504"/>
</node>
<node TEXT="orientation" ID="ID_1356605415" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="SIDE" ID="ID_593040061" CREATED="1433964260806" MODIFIED="1434316234833"/>
</node>
<node TEXT="spacing" ID="ID_894677213" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="3" OBJECT="java.lang.Long|3" ID="ID_1985704102" CREATED="1434316247486" MODIFIED="1435418041931"/>
</node>
<node TEXT="status" ID="ID_1300106444" CREATED="1433963019992" MODIFIED="1433963023836">
<node TEXT="OPENED" ID="ID_1974302622" CREATED="1433963025262" MODIFIED="1433963036572"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_996556765" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_561999147" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door6&quot;" ID="ID_739383763" CREATED="1437598314952" MODIFIED="1437598390931"/>
</node>
<node TEXT="pos" ID="ID_970062126" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="25" OBJECT="java.lang.Long|25" ID="ID_1321465967" CREATED="1433962916237" MODIFIED="1435418092657"/>
<node TEXT="35" OBJECT="java.lang.Long|35" ID="ID_1327430027" CREATED="1433962906568" MODIFIED="1435418095841"/>
</node>
<node TEXT="orientation" ID="ID_896120515" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="SIDE" ID="ID_895718289" CREATED="1433964260806" MODIFIED="1434316234833"/>
</node>
<node TEXT="spacing" ID="ID_1110567806" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_351478572" CREATED="1434316247486" MODIFIED="1435418098047"/>
</node>
<node TEXT="status" ID="ID_944447324" CREATED="1433963019992" MODIFIED="1433963023836">
<node TEXT="OPENED" ID="ID_1506949631" CREATED="1433963025262" MODIFIED="1433963036572"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_1098879021" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_1774753243" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door7&quot;" ID="ID_691780780" CREATED="1437598314952" MODIFIED="1437598401368"/>
</node>
<node TEXT="pos" ID="ID_375078927" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="24" OBJECT="java.lang.Long|24" ID="ID_123999981" CREATED="1433962916237" MODIFIED="1435418136299"/>
<node TEXT="29" OBJECT="java.lang.Long|29" ID="ID_438384490" CREATED="1433962906568" MODIFIED="1435418128802"/>
</node>
<node TEXT="orientation" ID="ID_1626328617" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_10243643" CREATED="1433964260806" MODIFIED="1435418141715"/>
</node>
<node TEXT="spacing" ID="ID_1757222869" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="1" OBJECT="java.lang.Long|1" ID="ID_35438719" CREATED="1434316247486" MODIFIED="1435418150761"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_666778419" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_795826511" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door8&quot;" ID="ID_673190733" CREATED="1437598314952" MODIFIED="1437598414710"/>
</node>
<node TEXT="pos" ID="ID_652997024" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="36" OBJECT="java.lang.Long|36" ID="ID_1964664086" CREATED="1433962916237" MODIFIED="1435418455798"/>
<node TEXT="31" OBJECT="java.lang.Long|31" ID="ID_243513599" CREATED="1433962906568" MODIFIED="1435418457946"/>
</node>
<node TEXT="orientation" ID="ID_1889481380" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="SIDE" ID="ID_721056377" CREATED="1433964260806" MODIFIED="1435418462231"/>
</node>
<node TEXT="spacing" ID="ID_893370675" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_199089194" CREATED="1434316247486" MODIFIED="1435418463659"/>
</node>
<node TEXT="unlockSide" ID="ID_816964738" CREATED="1433964010025" MODIFIED="1434318083287">
<node TEXT="NONE" ID="ID_590852270" CREATED="1433962775922" MODIFIED="1435420302687"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_1756002625" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_271767825" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door9&quot;" ID="ID_1667100361" CREATED="1437598314952" MODIFIED="1437598641408"/>
</node>
<node TEXT="pos" ID="ID_1108281208" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="29" OBJECT="java.lang.Long|29" ID="ID_1255600680" CREATED="1433962916237" MODIFIED="1435418487910"/>
<node TEXT="35" OBJECT="java.lang.Long|35" ID="ID_1532069775" CREATED="1433962906568" MODIFIED="1435418490111"/>
</node>
<node TEXT="orientation" ID="ID_614922618" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="SIDE" ID="ID_171371504" CREATED="1433964260806" MODIFIED="1435418462231"/>
</node>
<node TEXT="spacing" ID="ID_361920906" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_764271318" CREATED="1434316247486" MODIFIED="1435418463659"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_1068867980" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_854864459" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door10&quot;" ID="ID_3766770" CREATED="1437598314952" MODIFIED="1437598668931"/>
</node>
<node TEXT="pos" ID="ID_1989226719" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="32" OBJECT="java.lang.Long|32" ID="ID_897625690" CREATED="1433962916237" MODIFIED="1435420485096"/>
<node TEXT="22" OBJECT="java.lang.Long|22" ID="ID_910901801" CREATED="1433962906568" MODIFIED="1435420912011"/>
</node>
<node TEXT="orientation" ID="ID_1851305728" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_1479386083" CREATED="1433964260806" MODIFIED="1435420490135"/>
</node>
<node TEXT="spacing" ID="ID_1885637339" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_1151726899" CREATED="1434316247486" MODIFIED="1435418463659"/>
</node>
<node TEXT="status" ID="ID_1938361275" CREATED="1433963019992" MODIFIED="1433963023836">
<node TEXT="OPENED" ID="ID_1038280611" CREATED="1433963025262" MODIFIED="1433963036572"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_1846390785" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_1557538978" CREATED="1435487481325" MODIFIED="1435487483030">
<node TEXT="&quot;door11&quot;" ID="ID_429046240" CREATED="1435487484200" MODIFIED="1437598677249"/>
</node>
<node TEXT="pos" ID="ID_1822960255" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="32" OBJECT="java.lang.Long|32" ID="ID_1825507383" CREATED="1433962916237" MODIFIED="1435420485096"/>
<node TEXT="8" OBJECT="java.lang.Long|8" ID="ID_1787848095" CREATED="1433962906568" MODIFIED="1435420521942"/>
</node>
<node TEXT="orientation" ID="ID_954621036" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_1765168647" CREATED="1433964260806" MODIFIED="1435420490135"/>
</node>
<node TEXT="spacing" ID="ID_1468700149" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_40338258" CREATED="1434316247486" MODIFIED="1435418463659"/>
</node>
<node TEXT="unlockSide" ID="ID_1757412830" CREATED="1433964010025" MODIFIED="1434318083287">
<node TEXT="NONE" ID="ID_1799854150" CREATED="1433962775922" MODIFIED="1435420302687"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_845555641" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_1166397244" CREATED="1435487481325" MODIFIED="1435487483030">
<node TEXT="&quot;door12&quot;" ID="ID_1036237645" CREATED="1435487484200" MODIFIED="1437598564033"/>
</node>
<node TEXT="pos" ID="ID_696766704" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="25" OBJECT="java.lang.Long|25" ID="ID_1021482992" CREATED="1433962916237" MODIFIED="1435420572618"/>
<node TEXT="15" OBJECT="java.lang.Long|15" ID="ID_725019283" CREATED="1433962906568" MODIFIED="1435420574025"/>
</node>
<node TEXT="orientation" ID="ID_206422098" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="SIDE" ID="ID_261648950" CREATED="1433964260806" MODIFIED="1435420583323"/>
</node>
<node TEXT="spacing" ID="ID_237788446" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="3" OBJECT="java.lang.Long|3" ID="ID_1111201029" CREATED="1434316247486" MODIFIED="1435420585672"/>
</node>
<node TEXT="unlockSide" ID="ID_653644853" CREATED="1433964010025" MODIFIED="1434318083287">
<node TEXT="NONE" ID="ID_839505726" CREATED="1433962775922" MODIFIED="1435420302687"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_1666398743" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_1157407621" CREATED="1435487481325" MODIFIED="1435487483030">
<node TEXT="&quot;door13&quot;" ID="ID_669286956" CREATED="1435487484200" MODIFIED="1437598571220"/>
</node>
<node TEXT="pos" ID="ID_17176543" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="40" OBJECT="java.lang.Long|40" ID="ID_1226636145" CREATED="1433962916237" MODIFIED="1435420576632"/>
<node TEXT="15" OBJECT="java.lang.Long|15" ID="ID_1496833176" CREATED="1433962906568" MODIFIED="1435420578790"/>
</node>
<node TEXT="orientation" ID="ID_683007048" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="SIDE" ID="ID_1395809471" CREATED="1433964260806" MODIFIED="1435420593899"/>
</node>
<node TEXT="spacing" ID="ID_1375512429" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="3" OBJECT="java.lang.Long|3" ID="ID_19112984" CREATED="1434316247486" MODIFIED="1435420589776"/>
</node>
<node TEXT="unlockSide" ID="ID_1424381337" CREATED="1433964010025" MODIFIED="1434318083287">
<node TEXT="NONE" ID="ID_693638458" CREATED="1433962775922" MODIFIED="1435420302687"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_10915963" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_641033841" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door14&quot;" ID="ID_409500834" CREATED="1437598314952" MODIFIED="1437598578859"/>
</node>
<node TEXT="pos" ID="ID_579839642" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="17" OBJECT="java.lang.Long|17" ID="ID_652867023" CREATED="1433962916237" MODIFIED="1435420698754"/>
<node TEXT="7" OBJECT="java.lang.Long|7" ID="ID_393501434" CREATED="1433962906568" MODIFIED="1435420700543"/>
</node>
<node TEXT="orientation" ID="ID_1279550261" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_290178881" CREATED="1433964260806" MODIFIED="1435420964620"/>
</node>
<node TEXT="spacing" ID="ID_1488338798" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="1" OBJECT="java.lang.Long|1" ID="ID_1452470149" CREATED="1434316247486" MODIFIED="1435420967464"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_1933545522" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_1009174100" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door15&quot;" ID="ID_1023261624" CREATED="1437598314952" MODIFIED="1437598594498"/>
</node>
<node TEXT="pos" ID="ID_1135538001" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="5" OBJECT="java.lang.Long|5" ID="ID_1584969048" CREATED="1433962916237" MODIFIED="1435420694510"/>
<node TEXT="7" OBJECT="java.lang.Long|7" ID="ID_345949364" CREATED="1433962906568" MODIFIED="1435420696276"/>
</node>
<node TEXT="orientation" ID="ID_1057930700" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_286075750" CREATED="1433964260806" MODIFIED="1435420964620"/>
</node>
<node TEXT="spacing" ID="ID_82581727" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="1" OBJECT="java.lang.Long|1" ID="ID_728742424" CREATED="1434316247486" MODIFIED="1435420967464"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_380307880" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_1639707309" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door16&quot;" ID="ID_1693514086" CREATED="1437598314952" MODIFIED="1437598603327"/>
</node>
<node TEXT="pos" ID="ID_233968269" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="5" OBJECT="java.lang.Long|5" ID="ID_1199942690" CREATED="1433962916237" MODIFIED="1435420714281"/>
<node TEXT="11" OBJECT="java.lang.Long|11" ID="ID_756189296" CREATED="1433962906568" MODIFIED="1435420716201"/>
</node>
<node TEXT="orientation" ID="ID_158682253" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_652154249" CREATED="1433964260806" MODIFIED="1435420964620"/>
</node>
<node TEXT="spacing" ID="ID_1145558741" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="1" OBJECT="java.lang.Long|1" ID="ID_145369464" CREATED="1434316247486" MODIFIED="1435420967464"/>
</node>
<node TEXT="status" ID="ID_1832496698" CREATED="1433963019992" MODIFIED="1433963023836">
<node TEXT="OPENED" ID="ID_1399797762" CREATED="1433963025262" MODIFIED="1433963036572"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_1242555756" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_1614370311" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door17&quot;" ID="ID_912807917" CREATED="1437598314952" MODIFIED="1437598613799"/>
</node>
<node TEXT="pos" ID="ID_449096512" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="17" OBJECT="java.lang.Long|17" ID="ID_1041632897" CREATED="1433962916237" MODIFIED="1435420718668"/>
<node TEXT="11" OBJECT="java.lang.Long|11" ID="ID_671019684" CREATED="1433962906568" MODIFIED="1435420720638"/>
</node>
<node TEXT="orientation" ID="ID_1486128240" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="FACE" ID="ID_312353167" CREATED="1433964260806" MODIFIED="1435420964620"/>
</node>
<node TEXT="spacing" ID="ID_653954881" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="1" OBJECT="java.lang.Long|1" ID="ID_222654321" CREATED="1434316247486" MODIFIED="1435420967464"/>
</node>
</node>
<node TEXT="DOOR" POSITION="right" ID="ID_829974612" CREATED="1433962857291" MODIFIED="1433962860807">
<node TEXT="id" ID="ID_41054040" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;door18&quot;" ID="ID_1831674476" CREATED="1437598314952" MODIFIED="1437598621436"/>
</node>
<node TEXT="pos" ID="ID_909183137" CREATED="1433962861337" MODIFIED="1433962864253">
<node TEXT="44" OBJECT="java.lang.Long|44" ID="ID_570566063" CREATED="1433962916237" MODIFIED="1435421152005"/>
<node TEXT="9" OBJECT="java.lang.Long|9" ID="ID_1887851231" CREATED="1433962906568" MODIFIED="1435421154783"/>
</node>
<node TEXT="orientation" ID="ID_1841312904" CREATED="1433964251597" MODIFIED="1433964257744">
<node TEXT="SIDE" ID="ID_1396880630" CREATED="1433964260806" MODIFIED="1435420593899"/>
</node>
<node TEXT="spacing" ID="ID_1409382238" CREATED="1434316241151" MODIFIED="1434316247047">
<node TEXT="3" OBJECT="java.lang.Long|3" ID="ID_1161947540" CREATED="1434316247486" MODIFIED="1435420589776"/>
</node>
<node TEXT="requireItem" ID="ID_1775785834" CREATED="1433963184440" MODIFIED="1456682640555">
<node TEXT="S_PASS_LABORATORY_1" ID="ID_566121368" CREATED="1433963416765" MODIFIED="1457137427085"/>
</node>
</node>
<node TEXT="NPC" POSITION="right" ID="ID_125662742" CREATED="1435610691037" MODIFIED="1436003653120"><richcontent TYPE="NOTE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      TEST
    </p>
  </body>
</html>
</richcontent>
<node TEXT="id" ID="ID_1193862627" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;npc test&quot;" ID="ID_677747121" CREATED="1437598314952" MODIFIED="1437598445832"/>
</node>
<node TEXT="pos" ID="ID_663382366" CREATED="1435693608022" MODIFIED="1435693609780">
<node TEXT="6" OBJECT="java.lang.Long|6" ID="ID_860362536" CREATED="1435693618757" MODIFIED="1435693620446"/>
<node TEXT="24" OBJECT="java.lang.Long|24" ID="ID_411217721" CREATED="1435693610636" MODIFIED="1457891831798"/>
</node>
<node TEXT="level" ID="ID_189838579" CREATED="1458409368293" MODIFIED="1458409409603">
<node TEXT="10" OBJECT="java.lang.Long|10" ID="ID_312499514" CREATED="1458409384117" MODIFIED="1458495196830"/>
</node>
</node>
<node TEXT="NPC" POSITION="right" ID="ID_1431939490" CREATED="1435610691037" MODIFIED="1435610695935">
<node TEXT="id" ID="ID_838295912" CREATED="1437598457603" MODIFIED="1437598458392">
<node TEXT="&quot;npc1&quot;" ID="ID_1969574844" CREATED="1437598314952" MODIFIED="1437598466833"/>
</node>
<node TEXT="pos" ID="ID_422803618" CREATED="1435693608022" MODIFIED="1435693609780">
<node TEXT="3" OBJECT="java.lang.Long|3" ID="ID_813411768" CREATED="1435693618757" MODIFIED="1436003683584"/>
<node TEXT="16" OBJECT="java.lang.Long|16" ID="ID_1079125923" CREATED="1435693610636" MODIFIED="1436003686747"/>
</node>
<node TEXT="level" ID="ID_490230779" CREATED="1458409368293" MODIFIED="1458409409603">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_1760338604" CREATED="1458409384117" MODIFIED="1458409415618"/>
</node>
</node>
<node TEXT="NPC" POSITION="right" ID="ID_1691534208" CREATED="1435610691037" MODIFIED="1435610695935">
<node TEXT="id" ID="ID_653720529" CREATED="1437598457603" MODIFIED="1437598458392">
<node TEXT="&quot;npc2&quot;" ID="ID_1329564973" CREATED="1437598314952" MODIFIED="1437598476080"/>
</node>
<node TEXT="pos" ID="ID_937814252" CREATED="1435693608022" MODIFIED="1435693609780">
<node TEXT="45" OBJECT="java.lang.Long|45" ID="ID_1727376511" CREATED="1435693618757" MODIFIED="1436003705271"/>
<node TEXT="24" OBJECT="java.lang.Long|24" ID="ID_1304950553" CREATED="1435693610636" MODIFIED="1436003707546"/>
</node>
<node TEXT="level" ID="ID_422390466" CREATED="1458409368293" MODIFIED="1458409409603">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_1781135375" CREATED="1458409384117" MODIFIED="1458409415618"/>
</node>
</node>
<node TEXT="NPC" POSITION="right" ID="ID_809028684" CREATED="1435610691037" MODIFIED="1435610695935">
<node TEXT="id" ID="ID_565595271" CREATED="1437598457603" MODIFIED="1437598458392">
<node TEXT="&quot;npc3&quot;" ID="ID_527006239" CREATED="1437598314952" MODIFIED="1438807941890"/>
</node>
<node TEXT="pos" ID="ID_472128738" CREATED="1435693608022" MODIFIED="1435693609780">
<node TEXT="42" OBJECT="java.lang.Long|42" ID="ID_602723366" CREATED="1435693618757" MODIFIED="1438807946062"/>
<node TEXT="31" OBJECT="java.lang.Long|31" ID="ID_567743510" CREATED="1435693610636" MODIFIED="1438807948612"/>
</node>
<node TEXT="level" ID="ID_1989544115" CREATED="1458409368293" MODIFIED="1458409409603">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_1763079229" CREATED="1458409384117" MODIFIED="1458409415618"/>
</node>
</node>
<node TEXT="LOOT" POSITION="left" ID="ID_1287707956" CREATED="1452989475209" MODIFIED="1456680501547"><richcontent TYPE="NOTE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      TEST
    </p>
  </body>
</html>
</richcontent>
<node TEXT="id" ID="ID_290132714" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;loot1&quot;" ID="ID_1733293394" CREATED="1437598314952" MODIFIED="1452995546765"/>
</node>
<node TEXT="pos" ID="ID_1894702582" CREATED="1432405149698" MODIFIED="1433598624484">
<node TEXT="6" OBJECT="java.lang.Long|6" ID="ID_153689437" CREATED="1432405166477" MODIFIED="1456666915845"/>
<node TEXT="27" OBJECT="java.lang.Long|27" ID="ID_503410200" CREATED="1432407013415" MODIFIED="1456666918002"/>
</node>
<node TEXT="item" ID="ID_1855471253" CREATED="1456680689244" MODIFIED="1456680691643">
<node TEXT="W_KATANA" ID="ID_1506883160" CREATED="1456680693346" MODIFIED="1457137368005"/>
</node>
</node>
<node TEXT="LOOT" POSITION="left" ID="ID_900638681" CREATED="1452989475209" MODIFIED="1456680501547"><richcontent TYPE="NOTE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      TEST
    </p>
  </body>
</html>
</richcontent>
<node TEXT="id" ID="ID_1484631125" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;loot2&quot;" ID="ID_359656628" CREATED="1437598314952" MODIFIED="1456680984711"/>
</node>
<node TEXT="pos" ID="ID_1710340008" CREATED="1432405149698" MODIFIED="1433598624484">
<node TEXT="7" OBJECT="java.lang.Long|7" ID="ID_1289073264" CREATED="1432405166477" MODIFIED="1456687626933"/>
<node TEXT="27" OBJECT="java.lang.Long|27" ID="ID_1739577724" CREATED="1432407013415" MODIFIED="1456687614757"/>
</node>
<node TEXT="item" ID="ID_826291376" CREATED="1456680689244" MODIFIED="1456680691643">
<node TEXT="O_ARMY_GRENADE" ID="ID_574251311" CREATED="1456680693346" MODIFIED="1457137361421"/>
</node>
<node TEXT="nbGrouped" ID="ID_156202108" CREATED="1456681040443" MODIFIED="1456681081791">
<node TEXT="10" OBJECT="java.lang.Long|10" ID="ID_497582612" CREATED="1456681082527" MODIFIED="1456681085074"/>
</node>
</node>
<node TEXT="LOOT" POSITION="left" ID="ID_1663122246" CREATED="1452989475209" MODIFIED="1456680501547"><richcontent TYPE="NOTE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      TEST
    </p>
  </body>
</html>
</richcontent>
<node TEXT="id" ID="ID_1200246270" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;loot3&quot;" ID="ID_1236652841" CREATED="1437598314952" MODIFIED="1457744231057"/>
</node>
<node TEXT="pos" ID="ID_1747619748" CREATED="1432405149698" MODIFIED="1433598624484">
<node TEXT="2" OBJECT="java.lang.Long|2" ID="ID_637060737" CREATED="1432405166477" MODIFIED="1457744289440"/>
<node TEXT="27" OBJECT="java.lang.Long|27" ID="ID_937334421" CREATED="1432407013415" MODIFIED="1456687614757"/>
</node>
<node TEXT="item" ID="ID_469301788" CREATED="1456680689244" MODIFIED="1456680691643">
<node TEXT="W_ARMY_SHIELD" ID="ID_1496501317" CREATED="1456680693346" MODIFIED="1457744266774"/>
</node>
</node>
<node TEXT="LOOT" POSITION="left" ID="ID_535155536" CREATED="1452989475209" MODIFIED="1456680501547"><richcontent TYPE="NOTE">

<html>
  <head>
    
  </head>
  <body>
    <p>
      TEST
    </p>
  </body>
</html>
</richcontent>
<node TEXT="id" ID="ID_1759738692" CREATED="1437598312285" MODIFIED="1437598314125">
<node TEXT="&quot;loot pass&quot;" ID="ID_311972242" CREATED="1437598314952" MODIFIED="1457380443241"/>
</node>
<node TEXT="pos" ID="ID_756304163" CREATED="1432405149698" MODIFIED="1433598624484">
<node TEXT="23" OBJECT="java.lang.Long|23" ID="ID_735165829" CREATED="1432405166477" MODIFIED="1457382422601"/>
<node TEXT="35" OBJECT="java.lang.Long|35" ID="ID_1440640643" CREATED="1432407013415" MODIFIED="1457382424900"/>
</node>
<node TEXT="item" ID="ID_785956838" CREATED="1456680689244" MODIFIED="1456680691643">
<node TEXT="S_PASS_LABORATORY_1" ID="ID_708313988" CREATED="1433963416765" MODIFIED="1457137427085"/>
</node>
</node>
</node>
</map>
