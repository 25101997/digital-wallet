# 📘 Guía de Trabajo con Git

Esta guía define la configuración inicial, la convención de ramas y commits, y el flujo de integración para proyectos en Git.

---

## 🚀 Inicialización del repositorio

```bash
# Crear repositorio local y enlazar con remoto
git init
git branch -M main
git remote add origin https://github.com/25101997/nd18-ag19-dn8-psql16.git

# Primer commit
git add .
git commit -m "proyecto base totalmente funcional"
git push -u origin main

# Verificar y actualizar URL remota
git remote set-url origin https://github.com/25101997/nd18-ag19-dn8-psql16.git
git remote -v

    💡 Si el repositorio ya existe en GitHub, es más sencillo clonarlo directamente:

bash

git clone https://github.com/25101997/nd18-ag19-dn8-psql16.git

🌿 Convención de ramas

    main → rama estable, siempre desplegable en producción.

    develop → rama de integración, donde se consolidan features antes de pasar a producción.

    feature/ → nuevas funcionalidades. Ejemplo: feature/login.

    bugfix/ → corrección de errores menores. Ejemplo: bugfix/404-error.

    hotfix/ → parches urgentes en producción. Ejemplo: hotfix/security-patch.

    release/ → preparación de versiones. Ejemplo: release/v1.2.0.

    chore/ → tareas menores. Ejemplo: chore/update-docs.

📝 Convención de commits (Conventional Commits)

Usar prefijos claros para cada tipo de cambio:

    feat: nueva funcionalidad

    fix: corrección de bug

    refactor: cambios internos sin alterar funcionalidad

    docs: documentación

    style: cambios de formato (indentación, espacios, comillas)

    ci: configuración de integración continua

    build: cambios en build, dependencias o infraestructura

    test: pruebas unitarias o de integración

    chore: tareas menores

    devops: configuración de archivos

Ejemplo:
bash

git commit -m "feat: agregar login con JWT"

🔄 Flujo de integración y despliegue
Integrar ramas en develop
bash

git checkout develop
git pull origin develop
git merge feature/login
git push origin develop

Integrar develop en main
bash

git checkout main
git pull origin main
git merge develop
git push origin main

    ✅ Recomendación: usar Pull Requests (PRs) en GitHub para revisión de código antes de hacer merge a develop o main.

📌 Buenas prácticas

    Mantener main siempre estable y listo para producción.

    Usar develop como rama de integración.

    Crear ramas específicas para cada feature, bugfix o hotfix.

    Hacer commits pequeños y descriptivos siguiendo la convención.

    Revisar y aprobar PRs antes de mergear a ramas principales.