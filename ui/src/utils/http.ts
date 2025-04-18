import axios from 'axios'

export const HTTP = axios.create({
  baseURL: import.meta.env.VITE_BASE_URL,
  withCredentials: true,
})

HTTP.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config

    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true

      await HTTP.post('/api/auth/refresh-token')
      return HTTP(originalRequest)
    }

    return Promise.reject(error)
  },
)
