SELECT DISTINCT R.target AS target, json_object(
    'validateurProfilID', R.value,
    'validateur', (WITH CTE AS (
        SELECT DISTINCT json_extract(JSON, '$.validateur') AS validateur, record.value AS profil_id
        FROM record, json_each(record.description) AS JSON
        WHERE traitement='ValidateurAbsent' OR traitement='ValidationInterne'
        )
        SELECT CTE.validateur
        FROM CTE
        JOIN record AS R1 ON R1.value=CTE.profil_id
        WHERE R1.value=R.value
    ),
    'validateurProfilType', (WITH CTE AS (
        SELECT DISTINCT json_extract(JSON, '$.typeProfil') AS typeProfil, record.value AS profil_id
        FROM record, json_each(record.description) AS JSON
        WHERE traitement='ValidateurAbsent' OR traitement='ValidationInterne'
        )
        SELECT CTE.typeProfil
        FROM CTE
        JOIN record AS R1 ON R1.value=CTE.profil_id
        WHERE R1.value=R.value
    ),
    'validateurAbsent',(
    CASE 
        WHEN traitement='ValidateurAbsent' THEN "true"
        ELSE "false"
    END),
    'validateurInterne',(
    CASE 
        WHEN traitement='ValidationInterne' THEN "true"
        ELSE "false"
    END)
    ) AS json
    FROM record AS R, json_each(R.description) AS JSON
    WHERE traitement='ValidateurAbsent' OR traitement='ValidationInterne'
    AND R.target=@Target
    AND date(R.date) BETWEEN date(@DateDebut) AND date(@DateFin)
    ;