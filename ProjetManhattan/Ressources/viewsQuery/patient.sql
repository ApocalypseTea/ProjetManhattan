SELECT DISTINCT R.target, json_object(
    'patientID', REPLACE(R.target, 'PatientID=' , ''),
    'ancienneDateNaissance', (WITH CTE AS (
                            SELECT DISTINCT json_extract(JSON, '$.previousDate') AS old_date, R.value AS new_date
                            FROM record AS R, json_each(R.description) AS JSON
                            WHERE traitement='ModificationDateNaissance')
                                SELECT old_date
                                FROM CTE
                                JOIN record AS R1 ON R1.value=CTE.new_date
                                WHERE R1.value=R.value),
    'nouvelleDateNaissance', R.value) AS json
FROM record AS R
WHERE traitement='ModificationDateNaissance'
AND R.target=@Target
AND date(R.date) BETWEEN date(@DateDebut) AND date(@DateFin);