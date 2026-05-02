CREATE TABLE dw.movimiento (
    id_movimiento SERIAL PRIMARY KEY,

    id_cuenta INT NOT NULL,

    tipo VARCHAR(20) NOT NULL,
    monto NUMERIC(14,2) NOT NULL,

    descripcion VARCHAR(255),

    mes INT,
    anio INT,

    via VARCHAR(50) NOT NULL,

    id_cuenta_origen INT,
    id_cuenta_destino INT,

    created TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_movimiento_cuenta
        FOREIGN KEY (id_cuenta)
        REFERENCES dw.cuenta(id_cuenta),

    CONSTRAINT fk_movimiento_cuenta_origen
        FOREIGN KEY (id_cuenta_origen)
        REFERENCES dw.cuenta(id_cuenta),

    CONSTRAINT fk_movimiento_cuenta_destino
        FOREIGN KEY (id_cuenta_destino)
        REFERENCES dw.cuenta(id_cuenta),

    CONSTRAINT chk_movimiento_tipo
        CHECK (tipo IN ('debitar', 'acreditar')),

    CONSTRAINT chk_movimiento_monto
        CHECK (monto > 0),

    CONSTRAINT chk_movimiento_mes
        CHECK (mes BETWEEN 1 AND 12),

    CONSTRAINT chk_movimiento_anio
        CHECK (anio BETWEEN 2000 AND 2100),

    CONSTRAINT chk_movimiento_via
        CHECK (via IN ('efectivo', 'transferencia', 'deposito', 'retiro', 'ajuste'))
);