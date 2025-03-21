ProjetManhattan
	- ttt (command)
		- --nomTraitement (option required)
			- *StatIp
			- *TempsRequete
			- *URL
			- *BrisGlace
			- *ValidationInterne
			- *ChangementIdentite
			- *ValidateurAbsent
		- --nomOutput (option)
			- *bd (default)
				- *nomBaseDonnée (arg) (default = resultatTraitement)
				(pas besoin de mettre d'extension de fichier, ajout de .db automatique)
			- *console
	- toZabbix (command)
			Note : L'ordre entre option et argument n'a pas d'importance ;)
		- *nomBaseDonnee (arg) 
			- *resultatTraitement.db (default)
		- --nomTraitement (option required) 
			- *StatIp
			- *TempsRequete
			- *URL
			- *BrisGlace
			- *ValidationInterne
			- *ChangementIdentite
			- *ValidateurAbsent
			- *LocalisationIp
		- getValue (subcommand)
			- --nomTraitement (option required)
				- *BrisGlace
				- *StatIP
				- *TempsRequete
				- *URL
				- *ValidationInterne
				- *ChangementIdentite
				- *ValidateurAbsent

			- --nomTarget(option required)
				- * ProfilID:5881         (exemple pour Bris glace)
				- * 83.206.51.169         (exemple pour Stat Ip)
				- * /rcp/Home/ScriptGetPreferencesForCurrentUser (exemple pour Temps Requete)
				- * /cgi-bin/luci/;stok=/locale (exemple pour URL)
				- * FicheRCPID:36776      (exemple pour RCP Valide par Interne)
				- * userIDModified:27336  (exemple pour Changement Identite User)
				- * Validateur:2257       (exemple pour Validateur RCP absent)

			- --nomPropertyName(option required)
				- *NbBrisGlace (pour Bris glace)
				- *NbRequetes (pour Stat Ip)
				- *TimeTaken (pour Temps Requete)
				- *UrlDouteuse (pour Url)
				- *Validateur (pour RCP validé par Interne)
				- *Modificateur (pour Changement Identite User)
				- *"Validateur Absent" (pour pour Validateur RCP absent)

LEGENDE : *Options possibles pour le texte à ecrire

Base de données Source des localisation IP : https://lite.ip2location.com/database/db1-ip-country


