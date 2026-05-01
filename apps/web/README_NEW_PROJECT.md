web$ 
docker run -it --rm \
  -v "$PWD":/web \
  -u $(id -u):$(id -g) \
  -w /web \
  node:18-alpine sh

node -v
    v18.20.8

npm -v
    10.8.2

npx -p @angular/cli@19 ng new web --directory . --skip-git
npx ng --version
npm start -- --host 0.0.0.0 --port 4200 / npx ng serve --host 0.0.0.0 --port 4200


web$ 
docker run -it --rm -v "$PWD":/web -u $(id -u):$(id -g) -w /web -p 4200:4200 node:18-alpine sh

npm start -- --host 0.0.0.0 --port 4200 / npx ng serve --host 0.0.0.0 --port 4200



npx ng --version
npm install @angular/cli@19 --save-dev
rm -R node_modules package-lock.json package.json
npx ng --version
npx ng new . --directory .
npm install
npx ng serve --host 0.0.0.0 --port 4200






