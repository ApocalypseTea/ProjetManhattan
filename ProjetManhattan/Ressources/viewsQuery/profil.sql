SELECT DISTINCT R.target, json_object(
    'profilID', REPLACE(R.target,'ProfilID=','') ,
    'nbBrisGlace', R.value) AS json
FROM record AS R
WHERE R.target=@Target
AND
CONVERT(DATE, R.date) BETWEEN CONVERT(DATE, @DateDebut) AND CONVERT(DATE, @DateFin)
;