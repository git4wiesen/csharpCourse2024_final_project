# Abschlussprojekt für ein Visual C# Kurs mit Zertifikat

**Kursanbieter:** alfatraining Bildungszentrum GmbH\
**Kurswebsite:** https://www.alfatraining.de/kurse/weiterbildung-c-entwickler \
**Kurszeitraum:** 2. April 2024 bis 24. Mai 2024\
**Kursteilnehmer:** Christian Alexander Wiesenäcker\
**LinkedIn Profil:** www.linkedin.com/in/christianwiesenaecker

# Das Abschlussprojekt

**Bearbeitungslänge:** 2 Wochen\
**Finalisierung:** nicht fertig implementiert\
**Kurs-Abschluss:** Bestanden mit Zertifikat (100 von 100 Punkten)

## Lizenz: GPL v3
Dieses C# Abschlussprojekt ist unter der GPL v3 lizensiert.

## Abhängigkeiten

Auf dem Rechner muss der Microsoft.ACE.OLEDB.12.0 Provider installiert sein, damit diese App auf die Access-Datenbanken zugreifen kann.

Dazu muss eine der folgenden Installationspakete installiert werden, die diesen Provider standardmäßig enthalten:

- Microsoft Access (es ist keine valide Lizenz notwending)
- Microsoft Access Database Engine 2016 Redistributable (https://www.microsoft.com/de-de/download/details.aspx?id=54920)
- Microsoft 365 Access Runtime (https://support.microsoft.com/de-de/office/herunterladen-und-installieren-von-microsoft-365-access-runtime-185c5a32-8ba9-491e-ac76-91cbe3ea09c9)


## Release

**Download:** [Zip-Datei (175 MB)](https://drive.google.com/file/d/1MKUtVUKGna8gV3elOeoGjMGBvA91O-Vd/view?usp=sharing)

Die Zip-Datei enthält fertig gebaute Versionen dieses Abschlussprojekts.\
Die Anwendungen wurden so modifiziert, dass sie die zu nutzende Datenbank im selben Ausführungsverzeichnis sucht.

Die verwendete Datenbank ist immer 'schulplan_db_modify.accdb'. \
Die Datenbank-Datei 'schulplan_db_filled.accdb' ist mit Beispieldaten gefüllt und \
die Datenbank-Datei 'schulplan_db_empty.accdb' enthält nur die Datenbankstruktur aber keine Daten.


## Aufgabenstellung: Schulverwaltung zur Erstellung von Stundenplänen

Implementierung einer graphischen Schulverwaltung zur Erstellung von Schulstundenplänen.

**Datenbank:**
- Es werden Schüler und Lehrer erfasst und ihnen werden Personendaten zugeordnet.
- Schüler werden verschiedenen Klassen zugeordnet.
- Lehrer bekommen verschiedene Unterrichtsfächer zugeordnet
- Stundenplan-Einträgen
    - haben einen Schultag (Montag bis Freitag) und einen Zeitslot (8:00 - 8:45 / 8:45 - 9:30 / 9:45 - 10:30 / ...)
    - haben einen Lehrer, ein Unterrichtsfach, einen Lehrort sowie optional eine Klasse zugeordnet
- Zuordnung von Stundenplan-Einträgen zu Klassen und zu einzelnen Schülern,
 um Klassenstundenpläne sowie spezifische Schüler-Stundenpläne mit zusätzlichen Wahlfächern abzubilden


**Die Anwendung**
Zeigt die vorhandenen Klassen / Schüler / Lehrer an und ermöglicht das Hinzufügen / Editieren / Löschen von Einträgen.
Zeigt die Klassenstundenpläne aus einer vorgefüllten Datenbank an (die Editierung ist derzeit nicht möglich, aber die Benutzeroberfläche wurde angefangen)

**Unit Tests**
Es wurden Unit-Tests geschrieben, die dazu genutzt wurden, die benutzte Datenbank mit plausiblen Dummy-Daten zu füllen.

## Screenshots
<img src="screenshot/access_datenbank.png"> \
<img src="screenshot/klassen_schueler_lehrer.png"> \
<img src="screenshot/klassen_schueler_lehrer-klasse_editieren.png"> \
<img src="screenshot/klassen_schueler_lehrer-schueler_editieren.png"> \
<img src="screenshot/klassen_schueler_lehrer-lehrer_editieren.png"> \
<img src="screenshot/klassenstundenplan.png"> \
<img src="screenshot/klassenstundenplan-eintrag_editieren.png"> \
