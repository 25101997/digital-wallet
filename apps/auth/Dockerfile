FROM golang:1.25-alpine AS build

WORKDIR /app

COPY go.mod go.sum ./
RUN go mod download

COPY . .

RUN go build -o auth-service ./cmd/auth-service

FROM alpine:3.22

WORKDIR /app

COPY --from=build /app/auth-service .

EXPOSE 8081

ENTRYPOINT ["./auth-service"]