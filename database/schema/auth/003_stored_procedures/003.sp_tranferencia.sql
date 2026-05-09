CREATE OR REPLACE FUNCTION dw.transferir(
    p_id_cuenta_origen INT,
    p_id_cuenta_destino INT,
    p_monto NUMERIC,
    p_descripcion VARCHAR DEFAULT NULL
) 
RETURNS VOID AS $$
DECLARE
    v_saldo NUMERIC(14,2);
    v_existe_origen BOOLEAN;
    v_existe_destino BOOLEAN;
BEGIN
    -- Validar cuenta origen
    IF p_id_cuenta_origen IS NULL OR p_id_cuenta_origen <= 0 THEN
        RAISE EXCEPTION 'La cuenta origen es obligatoria';
    END IF;

    -- Validar cuenta destino
    IF p_id_cuenta_destino IS NULL OR p_id_cuenta_destino <= 0 THEN
        RAISE EXCEPTION 'La cuenta destino es obligatoria';
    END IF;

    -- Evitar transferir a la misma cuenta
    IF p_id_cuenta_origen = p_id_cuenta_destino THEN
        RAISE EXCEPTION 'No se puede transferir a la misma cuenta';
    END IF;

    -- Validar monto
    IF p_monto IS NULL OR p_monto <= 0 THEN
        RAISE EXCEPTION 'El monto debe ser mayor a cero';
    END IF;

    -- Bloquear cuenta origen para evitar doble gasto en operaciones simultáneas
    SELECT TRUE
    INTO v_existe_origen
    FROM dw.cuenta
    WHERE id_cuenta = p_id_cuenta_origen
      AND activa = TRUE
    FOR UPDATE;

    IF v_existe_origen IS NULL THEN
        RAISE EXCEPTION 'La cuenta origen no existe o está inactiva';
    END IF;

    -- Validar cuenta destino activa
    SELECT TRUE
    INTO v_existe_destino
    FROM dw.cuenta
    WHERE id_cuenta = p_id_cuenta_destino
      AND activa = TRUE;

    IF v_existe_destino IS NULL THEN
        RAISE EXCEPTION 'La cuenta destino no existe o está inactiva';
    END IF;

    -- Calcular saldo actual de la cuenta origen
    SELECT COALESCE(
        SUM(
            CASE 
                WHEN tipo = 'acreditar' THEN monto
                WHEN tipo = 'debitar'  THEN -monto
                ELSE 0
            END
        ), 0
    )
    INTO v_saldo
    FROM dw.movimiento
    WHERE id_cuenta = p_id_cuenta_origen;

    -- Validar saldo suficiente
    IF v_saldo < p_monto THEN
        RAISE EXCEPTION 
            'Saldo insuficiente. Saldo actual: %, monto solicitado: %',
            v_saldo, p_monto;
    END IF;

    -- Registrar débito en cuenta origen
    INSERT INTO dw.movimiento (
        id_cuenta,
        tipo,
        monto,
        descripcion,
        via,
        id_cuenta_origen,
        id_cuenta_destino
    )
    VALUES (
        p_id_cuenta_origen,
        'debitar',
        p_monto,
        COALESCE(p_descripcion, 'Transferencia enviada'),
        'transferencia',
        p_id_cuenta_origen,
        p_id_cuenta_destino
    );

    -- Registrar crédito en cuenta destino
    INSERT INTO dw.movimiento (
        id_cuenta,
        tipo,
        monto,
        descripcion,
        via,
        id_cuenta_origen,
        id_cuenta_destino
    )
    VALUES (
        p_id_cuenta_destino,
        'acreditar',
        p_monto,
        COALESCE(p_descripcion, 'Transferencia recibida'),
        'transferencia',
        p_id_cuenta_origen,
        p_id_cuenta_destino
    );

END;
$$ LANGUAGE plpgsql;

