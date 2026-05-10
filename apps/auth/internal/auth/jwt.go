package auth

import (
	"time"

	"github.com/golang-jwt/jwt/v5"
)

type JWTService struct {
	secret        string
	issuer        string
	audience      string
	accessMinutes int
}

type CustomClaims struct {
	UserID   string `json:"userId"`
	Username string `json:"username"`
	Email    string `json:"email"`
	jwt.RegisteredClaims
}

func NewJWTService(secret, issuer, audience string, accessMinutes int) *JWTService {
	return &JWTService{
		secret:        secret,
		issuer:        issuer,
		audience:      audience,
		accessMinutes: accessMinutes,
	}
}

func (s *JWTService) GenerateAccessToken(user User) (string, int, error) {
	expiresAt := time.Now().Add(time.Duration(s.accessMinutes) * time.Minute)

	claims := CustomClaims{
		UserID:   user.ID,
		Username: user.Username,
		Email:    user.Email,
		RegisteredClaims: jwt.RegisteredClaims{
			Issuer:    s.issuer,
			Subject:   user.ID,
			Audience:  []string{s.audience},
			ExpiresAt: jwt.NewNumericDate(expiresAt),
			IssuedAt:  jwt.NewNumericDate(time.Now()),
		},
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)

	signedToken, err := token.SignedString([]byte(s.secret))
	if err != nil {
		return "", 0, err
	}

	expiresIn := int(time.Until(expiresAt).Seconds())

	return signedToken, expiresIn, nil
}