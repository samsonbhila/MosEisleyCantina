# Use the official Node.js image as a build stage
FROM node:18 AS build
WORKDIR /app

# Copy package.json and package-lock.json files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the app's code
COPY . .

# Build the frontend
RUN npm run build

# Use a lightweight web server for production
FROM nginx:alpine AS production
WORKDIR /usr/share/nginx/html

# Copy built assets from the build stage
COPY --from=build /app/build .

# Copy custom Nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose the port the app will run on
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
