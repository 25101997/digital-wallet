package auth

import (
	"context"
	"errors"

	"github.com/jackc/pgx/v5/pgconn"
	"github.com/jackc/pgx/v5/pgxpool"
)

type Repository struct {
	db *pgxpool.Pool
}

func NewRepository(db *pgxpool.Pool) *Repository {
	return &Repository{
		db: db,
	}
}

func (r *Repository) CreateUser(ctx context.Context, req RegisterRequest, passwordHash string) (User, error) {
	query := `
		INSERT INTO auth.users (
			username,
			email,
			password_hash,
			full_name
		)
		VALUES ($1, $2, $3, $4)
		RETURNING 
			id,
			username,
			email,
			password_hash,
			full_name,
			is_active,
			created_at,
			updated_at;
	`

	var user User

	err := r.db.QueryRow(
		ctx,
		query,
		req.Username,
		req.Email,
		passwordHash,
		req.FullName,
	).Scan(
		&user.ID,
		&user.Username,
		&user.Email,
		&user.PasswordHash,
		&user.FullName,
		&user.IsActive,
		&user.CreatedAt,
		&user.UpdatedAt,
	)

	if err != nil {
		return User{}, mapPostgresError(err)
	}

	return user, nil
}

func (r *Repository) FindByUsernameOrEmail(ctx context.Context, usernameOrEmail string) (User, error) {
	query := `
		SELECT
			id,
			username,
			email,
			password_hash,
			full_name,
			is_active,
			created_at,
			updated_at
		FROM auth.users
		WHERE username = $1 OR email = $1
		LIMIT 1;
	`

	var user User

	err := r.db.QueryRow(
		ctx,
		query,
		usernameOrEmail,
	).Scan(
		&user.ID,
		&user.Username,
		&user.Email,
		&user.PasswordHash,
		&user.FullName,
		&user.IsActive,
		&user.CreatedAt,
		&user.UpdatedAt,
	)

	return user, err
}

func mapPostgresError(err error) error {
	var pgErr *pgconn.PgError

	if errors.As(err, &pgErr) {
		if pgErr.Code == "23505" {
			switch pgErr.ConstraintName {
			case "uq_auth_users_username":
				return ErrUsernameAlreadyExists
			case "uq_auth_users_email":
				return ErrEmailAlreadyExists
			default:
				return errors.New("registro duplicado")
			}
		}
	}

	return err
}