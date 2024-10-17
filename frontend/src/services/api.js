import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:7212/api',
});

api.interceptors.request.use((config) => {
    const user = JSON.parse(localStorage.getItem('user'));
    if (user && user.token) {
        config.headers.Authorization = `Bearer ${user.token}`;
    }
    return config;
}, (error) => {
    return Promise.reject(error);
});

export const fetchBooks = async (searchParams) => {
    try {
        const response = await api.get('/books', { params: searchParams });
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const createReservation = async (data) => {
    try {
        const response = await api.post('/reservations', data);
        return response.data;
    } catch (error) {
        throw error;
    }
};

export const fetchReservations = async (userId) => {
    try {
        const response = await api.get('/reservations', { params: { userId } });
        return response.data || [];
    } catch (error) {
        if (error.response && error.response.status === 404) {
            return [];
        }
        throw error.response?.data || 'Error fetching reservations';
    }
};

export const loginUser = async (credentials) => {
    try {
        const response = await api.post('/auth/login', credentials);
        return response.data;
    } catch (error) {
        throw error.response?.data || 'Login failed';
    }
};

export const registerUser = async (credentials) => {
    try {
        const response = await api.post('/auth/register', credentials);
        return response.data;
    } catch (error) {
        throw error.response?.data || 'Registration failed';
    }
};