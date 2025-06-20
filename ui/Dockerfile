FROM node:24-alpine3.22 AS build
WORKDIR /app
COPY /ui/package*.json ./
RUN npm install
COPY /ui/ .
RUN npm run build

FROM nginx:stable-alpine AS final
EXPOSE 8080
COPY --from=build /app/dist /usr/share/nginx/html
COPY ./ui/docker/nginx.conf /etc/nginx/conf.d/default.conf
CMD ["nginx", "-g", "daemon off;"]