package auth

import (
	"context"
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
	req.Email = strings.TrimSpace(strings.ToLower(req.Email))
	req.FullName = strings.TrimSpace(req.FullName)

	if err := validateRegisterRequest(req); err != nil {
		return RegisterResponse{}, err
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
	req.UsernameOrEmail = strings.TrimSpace(strings.ToLower(req.UsernameOrEmail))

	if err := validateLoginRequest(req); err != nil {
		return LoginResponse{}, err
	}

	user, err := s.repository.FindByUsernameOrEmail(ctx, req.UsernameOrEmail)
	if err != nil {
		return LoginResponse{}, ErrInvalidCredentials
	}

	if !user.IsActive {
		return LoginResponse{}, ErrUserInactive
	}

	if !CheckPassword(req.Password, user.PasswordHash) {
		return LoginResponse{}, ErrInvalidCredentials
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