-- ==========================================
-- Procedimiento: Listar cuentas
-- ==========================================
CREATE OR REPLACE FUNCTION dw.listar_cuentas()
RETURNS TABLE(
    id_cuenta INT,
    nombre VARCHAR,
    tipo VARCHAR,
    activa BOOLEAN,
    created TIMESTAMP,
    updated TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.id_cuenta,
        c.nombre,
        c.tipo,
        c.activa,
        c.created,
        c.updated
    FROM dw.cuenta c
    ORDER BY c.id_cuenta;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Obtener cuenta por id
-- ==========================================
CREATE OR REPLACE FUNCTION dw.obtener_cuenta_por_id(
    p_id INT
)
RETURNS TABLE(
    id_cuenta INT,
    nombre VARCHAR,
    tipo VARCHAR,
    activa BOOLEAN,
    created TIMESTAMP,
    updated TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        c.id_cuenta,
        c.nombre,
        c.tipo,
        c.activa,
        c.created,
        c.updated
    FROM dw.cuenta c
    WHERE c.id_cuenta = p_id;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Crear una nueva cuenta
-- ==========================================
CREATE OR REPLACE FUNCTION dw.crear_cuenta(
    p_nombre VARCHAR,
    p_tipo VARCHAR,
    p_activa BOOLEAN DEFAULT TRUE
) RETURNS INT AS $$
DECLARE
    v_id INT;
BEGIN
    INSERT INTO dw.cuenta(nombre, tipo, activa, created, updated)
    VALUES (p_nombre, p_tipo, p_activa, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
    RETURNING id_cuenta INTO v_id;

    RETURN v_id;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Actualizar cuenta
-- ==========================================
CREATE OR REPLACE FUNCTION dw.actualizar_cuenta(
    p_id_cuenta INT,
    p_nombre VARCHAR,
    p_tipo VARCHAR,
    p_activa BOOLEAN
) RETURNS VOID AS $$
BEGIN
    UPDATE dw.cuenta
    SET nombre = p_nombre,
        tipo = p_tipo,
        activa = p_activa,
        updated = CURRENT_TIMESTAMP
    WHERE id_cuenta = p_id_cuenta;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Activar cuenta
-- ==========================================
CREATE OR REPLACE FUNCTION dw.activar_cuenta(
    p_id_cuenta INT
) RETURNS VOID AS $$
BEGIN
    UPDATE dw.cuenta
    SET activa = TRUE,
        updated = CURRENT_TIMESTAMP
    WHERE id_cuenta = p_id_cuenta;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Desactivar cuenta
-- ==========================================
CREATE OR REPLACE FUNCTION dw.desactivar_cuenta(
    p_id_cuenta INT
) RETURNS VOID AS $$
BEGIN
    UPDATE dw.cuenta
    SET activa = FALSE,
        updated = CURRENT_TIMESTAMP
    WHERE id_cuenta = p_id_cuenta;
END;
$$ LANGUAGE plpgsql;
