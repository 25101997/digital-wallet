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
    RETURNING id INTO v_id;

    RETURN v_id;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Actualizar cuenta
-- ==========================================
CREATE OR REPLACE FUNCTION dw.actualizar_cuenta(
    p_id INT,
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
    WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Activar cuenta
-- ==========================================
CREATE OR REPLACE FUNCTION dw.activar_cuenta(
    p_id INT
) RETURNS VOID AS $$
BEGIN
    UPDATE dw.cuenta
    SET activa = TRUE,
        updated = CURRENT_TIMESTAMP
    WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Desactivar cuenta
-- ==========================================
CREATE OR REPLACE FUNCTION dw.desactivar_cuenta(
    p_id INT
) RETURNS VOID AS $$
BEGIN
    UPDATE dw.cuenta
    SET activa = FALSE,
        updated = CURRENT_TIMESTAMP
    WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;

-- ==========================================
-- Procedimiento: Eliminar cuenta
-- ==========================================
CREATE OR REPLACE FUNCTION dw.eliminar_cuenta(
    p_id INT
) RETURNS VOID AS $$
BEGIN
    DELETE FROM dw.cuenta WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;
