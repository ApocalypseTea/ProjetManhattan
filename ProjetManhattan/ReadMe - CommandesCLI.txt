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
				- *nomBaseDonnée (arg) (default = resultatTraitement )
			- *console
	- toZabbix (command)
		- *nomBaseDonnee (arg) 
			- *resultatTraitement.db (default)
		- getValue (subcommand)
			- --nomTraitement (option required)
				- *TraitementBrisGlace
				- *TraitementStatIP
				- *TraitementTempsRequete
				- *TraitementURL
				- *TraitementRCPvalideParInterne
				- *TraitementRequeteSQLChgtIdentiteUser
				- *FicheRCPAvecValidateurAbsent

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