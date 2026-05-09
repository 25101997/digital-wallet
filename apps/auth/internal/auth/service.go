package auth

import (
	"context"
	"errors"
	"strings"
)

type Service struct {
	repository *Repository
	jwtService *JWTService
}

func NewService(repository *Repository, jwtService *JWTService) *Service {
	return &Service{
		repository: repository,
		jwtService: jwtService,
	}
}

func (s *Service) Register(ctx context.Context, req RegisterRequest) (RegisterResponse, error) {
	req.Username = strings.TrimSpace(req.Username)
	req.Email = strings.TrimSpace(req.Email)
	req.FullName = strings.TrimSpace(req.FullName)

	if req.Username == "" {
		return RegisterResponse{}, errors.New("el username es obligatorio")
	}

	if req.Email == "" {
		return RegisterResponse{}, errors.New("el email es obligatorio")
	}

	if len(req.Password) < 8 {
		return RegisterResponse{}, errors.New("la contraseña debe tener al menos 8 caracteres")
	}

	passwordHash, err := HashPassword(req.Password)
	if err != nil {
		return RegisterResponse{}, err
	}

	user, err := s.repository.CreateUser(ctx, req, passwordHash)
	if err != nil {
		return RegisterResponse{}, err
	}

	return RegisterResponse{
		ID:       user.ID,
		Username: user.Username,
		Email:    user.Email,
		FullName: user.FullName,
	}, nil
}

func (s *Service) Login(ctx context.Context, req LoginRequest) (LoginResponse, error) {
	req.UsernameOrEmail = strings.TrimSpace(req.UsernameOrEmail)

	if req.UsernameOrEmail == "" {
		return LoginResponse{}, errors.New("usuario o email es obligatorio")
	}

	if req.Password == "" {
		return LoginResponse{}, errors.New("la contraseña es obligatoria")
	}

	user, err := s.repository.FindByUsernameOrEmail(ctx, req.UsernameOrEmail)
	if err != nil {
		return LoginResponse{}, errors.New("credenciales inválidas")
	}

	if !user.IsActive {
		return LoginResponse{}, errors.New("el usuario está inactivo")
	}

	if !CheckPassword(req.Password, user.PasswordHash) {
		return LoginResponse{}, errors.New("credenciales inválidas")
	}

	token, expiresIn, err := s.jwtService.GenerateAccessToken(user)
	if err != nil {
		return LoginResponse{}, err
	}

	return LoginResponse{
		AccessToken: token,
		TokenType:   "Bearer",
		ExpiresIn:   expiresIn,
	}, nil
}