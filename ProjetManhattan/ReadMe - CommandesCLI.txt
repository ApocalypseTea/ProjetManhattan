ProjetManhattan
	- --configFile (option required) (default = ..\..\..\Ressources\config.json")

	- traitement (COMMAND)
		- --traitement (option required)
		- --outputFormat (option required)
			- *bd
			- *console
		- --output (option required SI --outputFormat='bd') (nomdefichier avec ou sans extension .db)
		- --date (option)(default DateTime.Now)

	- toZabbix (COMMAND)
		- --input (option required) (nomdefichier avec ou sans extension .db)
		- --traitement (option required) 
	    [(- --debutPeriode (option)
		  - --finPeriode (option) (default DateTime.Now) )
		OU
		  - --date (option) ]

	- getValue (COMMAND)
		- --input (option required) (nomdefichier avec ou sans extension .db)
		- --traitement (option)
			OU
		- --propertyName(option)
			- *NbBrisGlace (pour Bris glace)
			- *NbRequetes (pour Stat Ip)
			- *TimeTaken (pour Temps Requete)
			- *UrlDouteuse (pour Url)
			- *Validateur (pour RCP validé par Interne)
			- *Modificateur (pour Changement Identite User)
			- *Validateur Absent (pour pour Validateur RCP absent)
			- *NouvelleDateNaissance (pour Modification Date de Naissance Patient)
			- *OrigineGeographique (pour Localisation Ip)
				
		- --target(option required)
			- * ProfilID:5881         (exemple pour Bris glace)
			- * 83.206.51.169         (exemple pour Stat Ip ou Localisation Ip)
			- * /rcp/Home/ScriptGetPreferencesForCurrentUser (exemple pour Temps Requete)
			- * /cgi-bin/luci/;stok=/locale (exemple pour URL)
			- * FicheRCPID:36776      (exemple pour RCP Valide par Interne)
			- * userIDModified:27336  (exemple pour Changement Identite User)
			- * Validateur:2257       (exemple pour Validateur RCP absent)

		- --date (option)(default DateTime.Now)
	
	- helpMe (COMMAND) Renvoie la liste des traitements disponibles à la date indiquée
		- --date (option)(default DateTime.Now)

///////////////////////////////////////////////////
LEGENDE : *Options possibles pour le texte à ecrire
///////////////////////////////////////////////////
Base de données Source des localisation IP : https://lite.ip2location.com/database/db1-ip-country


