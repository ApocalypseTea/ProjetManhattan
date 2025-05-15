SELECT DISTINCT R.target, json_object(
    'profilID', REPLACE(R.target,'ProfilID=','') ,
    'nbBrisGlace', R.value) AS json
FROM record AS R
WHERE R.target=@Target
AND
date(R.date) BETWEEN date(@DateDebut) AND date(@DateFin)
;