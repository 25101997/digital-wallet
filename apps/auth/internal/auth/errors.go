package auth

import "errors"

var (
	ErrUsernameRequired      = errors.New("el username es obligatorio")
	ErrEmailRequired         = errors.New("el email es obligatorio")
	ErrPasswordRequired      = errors.New("la contraseña es obligatoria")
	ErrUsernameOrEmailNeeded = errors.New("usuario o email es obligatorio")
	ErrWeakPassword          = errors.New("la contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un número")
	ErrInvalidCredentials    = errors.New("credenciales inválidas")
	ErrUserInactive          = errors.New("el usuario está inactivo")
	ErrUsernameAlreadyExists = errors.New("el username ya está registrado")
	ErrEmailAlreadyExists    = errors.New("el email ya está registrado")
)