git init
git branch -M main
git remote add origin https://github.com/25101997/nd18-ag19-dn8-psql16.git
git add .
git commit -m "proyecto base totalmente funcional"
git push -u origin main

git remote set-url origin https://github.com/25101997/nd18-ag19-dn8-psql16.git
git remote -v

git checkout -b rama

# Convención de Ramas
main → rama estable, siempre desplegable en producción.
develop → rama de integración, donde se consolidan features antes de pasar a producción.
feature/* → nuevas funcionalidades. Ejemplo: feature/login.
fix/* → correcciones menores. Ejemplo: fix/navbar-alignment.
hotfix/* → parches urgentes en producción. Ejemplo: hotfix/security-patch.

# Convención de commits
# Usar Conventional Commits con los siguientes prefijos:
feat: nueva funcionalidad.
fix: corrección de bug.
refactor: cambios internos sin alterar funcionalidad.
docs: documentación.
ci: configuración de integración continua.
build: cambios en build, dependencias o infraestructura.
test: pruebas unitarias o de integración.
devops: configuracion de archivos

# comandos mas usados para merge pull request

git checkout develop
git pull origin develop

git merge TodasLasRamasOsubramasCreadas

git push origin develop

git checkout main
git pull origin main
git merge develop
git push origin main

