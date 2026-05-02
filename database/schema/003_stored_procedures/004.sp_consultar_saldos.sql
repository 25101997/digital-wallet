CREATE OR REPLACE FUNCTION dw.consultar_saldos()
RETURNS TABLE(
    id_cuenta INT,
    nombre VARCHAR,
    saldo_actual NUMERIC
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.id_cuenta,
        c.nombre,
        COALESCE(SUM(
            CASE 
                WHEN m.tipo = 'acreditar' THEN m.monto
                WHEN m.tipo = 'debitar'  THEN -m.monto
                ELSE 0
            END
        ),0) AS saldo_actual
    FROM dw.cuenta c
    LEFT JOIN dw.movimiento m 
        ON c.id_cuenta = m.id_cuenta
    GROUP BY c.id_cuenta, c.nombre, c.tipo
    ORDER BY c.id_cuenta;
END;
$$ LANGUAGE plpgsql;
