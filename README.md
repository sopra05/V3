Vagram's Vicious Vengeance
======================

[For an english version see here.](README.en.md)

Dieses Videospiel ist im Rahmen des [Softwarepraktikums](https://sopra.informatik.uni-freiburg.de/) des [Lehrstuhls f�r Softwaretechnik](http://swt.informatik.uni-freiburg.de/) an der [Albert-Ludwigs-Universit�t Freiburg](http://www.uni-freiburg.de/) entstanden. Sechs zuf�llig zusammengew�rfelte Leute mit unterschiedlicher Erfahrung sa�en im Sommersemester 2016 zusammen und haben irgendwas zurechtgehackt.

Verwendet wurde das [MonoGame-Framework](http://www.monogame.net/) und eine Menge freier Sounds und Grafiken, die gesondert [in den CREDITS](V3/CREDITS.md) gelistet werden.

Die Steuerung erfolgt vorwiegend �ber die Maus und ist ziemlich konservativ. Dennoch gibt es eine [�bersicht �ber die m�glichen Befehle hier](V3/CONTROLS.md).

Zusammenfassung des Spiels
--------------------------

*"Pest und Verderben! Seid verflucht! Wie konntet Ihr mir das antun? Den Tod �ber eure Sippe! �ber euer Land! Mit eigenen H�nden werde ich dieses K�nigreich vernichten, und wenn es mich meine Seele kosten mag."*

Als ehemaliger Hofzauberer Vagrant kennt Ihr nur noch ein Ziel: Rache an K�nig Harry, der verantwortlich f�r den Tod Eurer Familie ist. Auf Euch allein gestellt ohne auch nur einen Verb�ndeten bleibt euch dazu nur ein Mittel: Die verbotene Kunst der Totenbeschw�rung. 

Pilgert durch das K�nigreich und hinterlasst eine Spur des Verw�stung. Entv�lkert ganze D�rfer und f�gt die wiederbelebten Kadaver Eurer Zombiearmee hinzu. Zerst�rt wo und was Ihr nur k�nnt und nutzt die �berbleibsel zur Verst�rkung Eurer willenlosen Streitkr�fte. Verschmelzt in unheiligen Ritualen Eure Kreaturen zu noch m�chtigeren Monstrosit�ten. �berrennt die verblendeten Vasallen des K�nigs mit euren Dienern aus Knochen und verwesendem Fleisch, denn sie haben es verdient. Jeder Nachkomme des verhassten K�nigs muss ausgemerzt werden um Euren Durst nach Rache zu stillen.

In dieser Mischung aus Action-RPG und Echtzeitstrategie geht es nicht um Aufbau und Eroberung. Kein Stein darf auf dem anderen bleiben, wollt Ihr Erfolg haben. Im Rahmen der Kampagne f�hrt ihr Kapitel f�r Kapitel das K�nigreich seinem Untergang entgegen. Und nun legt alle Skrupel ab, denn f�r Erl�sung ist es l�ngst zu sp�t.

Alleinstellungsmerkmale
-----------------------

Bei Vagrant's Vicious Vengeance soll eine offensive und risikoreiche Spielweise gef�rdert werden. Aus diesem Grund wird einerseits auf Aufbauelemente verzichtet. Einheiten werden aus Kadavern erschaffen, die durch die menschliche Bev�lkerung einer Karte begrenzt sind. Einheiten k�nnen ebenfalls verst�rkt werden, l�sst man sie Geb�ude zertr�mmern und die �berreste pl�ndern. Auch diese sind nur begrenzt vorhanden. 

Andererseits ist der Totenbeschw�rer selbst von zentraler Bedeutung: Ist er nicht anwesend, k�nnen die erschaffenen Einheiten nicht kontrolliert werden. Nur in seiner Gegenwart k�nnen Befehle erteilt und Spezialf�higkeiten aktiviert werden. Au�erdem k�nnen Einheiten kombiniert werden, um noch m�chtigere Zombies zu erschaffen. F�r sich selbst genommen besitzt der Totenbeschw�rer jedoch keine Angriffsm�glichkeiten, somit ist er also ohne beschworene Einheiten komplett wehrlos.

*Weiteres langweiliges Palaver darf gerne dem [Game Design Document](GDD.pdf) entnommen werden.*

Abh�ngigkeiten
-------------------

* [MonoGame 3.5](http://www.monogame.net/)
* .Net 4.5 / alternativ [Mono 4.4](http://www.mono-project.com/)
* [OpenAL](http://www.openal.org/)

Screenshots
-----------------

[![Screenshot 1](Screenshots/screen1_s.jpg)](Screenshots/screen1.jpg)

[![Screenshot 2](Screenshots/screen2_s.jpg)](Screenshots/screen2.jpg)

[![Screenshot 3](Screenshots/screen3_s.jpg)](Screenshots/screen3.jpg)

Bekannte Probleme
--------------------

* Unter Linux werden keine Umlaute angezeigt.
* Tempor�rer Framerateeinbruch, wenn man sehr vielen Einheiten auf einmal den Laufbefehl gibt.
