-- ==========================================
-- Procedimiento: Crear movimiento
-- ==========================================
CREATE OR REPLACE FUNCTION dw.crear_movimiento(
    p_id_cuenta INT,
    p_tipo VARCHAR,
    p_monto NUMERIC,
    p_descripcion VARCHAR,
    p_via VARCHAR
) RETURNS INT AS $$
DECLARE
    v_id INT;
BEGIN
    INSERT INTO dw.movimiento(
        id_cuenta, 
        tipo, 
        monto, 
        descripcion, 
        via, 
        created, 
        updated
    )
    VALUES (
        p_id_cuenta, 
        p_tipo, 
        p_monto, 
        p_descripcion, 
        p_via, 
        CURRENT_TIMESTAMP, 
        CURRENT_TIMESTAMP
    )
    RETURNING id_movimiento INTO v_id;

    RETURN v_id;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Actualizar movimiento
-- ==========================================
CREATE OR REPLACE FUNCTION dw.actualizar_movimiento(
    p_id_movimiento INT,
    p_id_cuenta INT,
    p_tipo VARCHAR,
    p_monto NUMERIC,
    p_descripcion VARCHAR,
    p_via VARCHAR
) RETURNS VOID AS $$
BEGIN
    UPDATE dw.movimiento
    SET id_cuenta = p_id_cuenta,
        tipo = p_tipo,
        monto = p_monto,
        descripcion = p_descripcion,
        via = p_via,
        updated = CURRENT_TIMESTAMP
    WHERE id_movimiento = p_id_movimiento;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Obtener movimiento por id
-- ==========================================
CREATE OR REPLACE FUNCTION dw.consultar_movimientos_por_id(
    p_id_movimiento INT
) RETURNS TABLE(
    id_movimiento INT,
    id_cuenta INT,
    nombre_cuenta VARCHAR,
    tipo VARCHAR,
    monto NUMERIC(14,2),
    descripcion VARCHAR,
    via VARCHAR,
    id_cuenta_origen INT,
    nombre_cuenta_origen VARCHAR,
    id_cuenta_destino INT,
    nombre_cuenta_destino VARCHAR,
    created TIMESTAMP,
    updated TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        m.id_movimiento,
        m.id_cuenta,
        cp.nombre AS nombre_cuenta,
        m.tipo,
        m.monto,
        m.descripcion,
        m.via,
        m.id_cuenta_origen,
        co.nombre AS nombre_cuenta_origen,
        m.id_cuenta_destino,
        cd.nombre AS nombre_cuenta_destino,
        m.created,
        m.updated
    FROM dw.movimiento m
    JOIN dw.cuenta cp ON m.id_cuenta = cp.id_cuenta
    LEFT JOIN dw.cuenta co ON m.id_cuenta_origen = co.id_cuenta
    LEFT JOIN dw.cuenta cd ON m.id_cuenta_destino = cd.id_cuenta
    WHERE m.id_movimiento = p_id_movimiento
    ORDER BY m.created DESC;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Obtener movimientos
-- ==========================================
CREATE OR REPLACE FUNCTION dw.consultar_movimientos() 
	RETURNS TABLE(
    id_movimiento INT,
    id_cuenta INT,
    tipo VARCHAR,
    monto NUMERIC(14,2),
    descripcion VARCHAR,
    via VARCHAR,
    id_cuenta_origen INT,
    id_cuenta_destino INT,
    created TIMESTAMP,
    updated TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        m.id_movimiento,
        m.id_cuenta,
        m.tipo,
        m.monto,
        m.descripcion,
        m.via,
        m.id_cuenta_origen,
        m.id_cuenta_destino,
        m.created,
        m.updated
    FROM dw.movimiento m
    ORDER BY m.created DESC;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Consultar movimientos por cuenta
-- ==========================================
CREATE OR REPLACE FUNCTION dw.consultar_movimientos_por_cuenta(
    p_id_cuenta INT
) RETURNS TABLE(
    id_movimiento INT,
    nombre_cuenta VARCHAR,
    tipo VARCHAR,
    monto NUMERIC(14,2),
    descripcion VARCHAR,
    via VARCHAR,
    id_cuenta_origen INT,
    nombre_cuenta_origen VARCHAR,
    id_cuenta_destino INT,
    nombre_cuenta_destino VARCHAR,
    created TIMESTAMP,
    updated TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        m.id_movimiento,
        cp.nombre AS nombre_cuenta,
        m.tipo,
        m.monto,
        m.descripcion,
        m.via,
        m.id_cuenta_origen,
        co.nombre AS nombre_cuenta_origen,
        m.id_cuenta_destino,
        cd.nombre AS nombre_cuenta_destino,
        m.created,
        m.updated
    FROM dw.movimiento m
    JOIN dw.cuenta cp ON m.id_cuenta = cp.id_cuenta
    LEFT JOIN dw.cuenta co ON m.id_cuenta_origen = co.id_cuenta
    LEFT JOIN dw.cuenta cd ON m.id_cuenta_destino = cd.id_cuenta
    WHERE m.id_cuenta = p_id_cuenta
    ORDER BY m.created DESC;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Eliminar movimiento
-- ==========================================
CREATE OR REPLACE FUNCTION dw.eliminar_movimiento(
    p_id_movimiento INT
) RETURNS VOID AS $$
BEGIN
    DELETE FROM dw.movimiento WHERE id_movimiento = p_id_movimiento;
END;
$$ LANGUAGE plpgsql;