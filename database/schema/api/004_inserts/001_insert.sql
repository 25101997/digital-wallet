INSERT INTO dw.cuenta (nombre, tipo, activa)
VALUES ('Banco Industrial', 'monetaria', TRUE);

INSERT INTO dw.cuenta (nombre, tipo, activa)
VALUES ('Banrural', 'monetaria', TRUE);

INSERT INTO dw.cuenta (nombre, tipo, activa)
VALUES ('Ahorro Local', 'ahorro', TRUE);

INSERT INTO dw.cuenta (nombre, tipo, activa)
VALUES ('Almuerzos', 'ahorro', TRUE);

INSERT INTO dw.cuenta (nombre, tipo, activa)
VALUES ('Desayunos Cenas', 'ahorro', TRUE);

INSERT INTO dw.cuenta (nombre, tipo, activa)
VALUES ('Gasolina', 'ahorro', TRUE);

INSERT INTO dw.cuenta (nombre, tipo, activa)
VALUES ('Diversion', 'ahorro', TRUE);

INSERT INTO dw.cuenta (nombre, tipo, activa)
VALUES ('Salud', 'ahorro', TRUE);

/*
SELECT 
    n.nspname AS schema,
    p.proname AS function_name,
    pg_get_function_identity_arguments(p.oid) AS argumentos
FROM pg_proc p
JOIN pg_namespace n 
    ON n.oid = p.pronamespace
WHERE n.nspname = 'dw'
  AND p.proname = 'transferir';
*/

-- DROP FUNCTION IF EXISTS dw.transferir(INT, INT, NUMERIC, VARCHAR, INT, INT, VARCHAR);

--SELECT * FROM dw.cuenta;
--SELECT * FROM dw.movimiento;

-- Consultar todos los saldos de cuentas
--SELECT * FROM dw.consultar_saldos();

--SELECT dw.transferir(2, 9, 225, 'Transferencia para gastos');

--SELECT * FROM dw.consultar_movimientos_por_cuenta(2);