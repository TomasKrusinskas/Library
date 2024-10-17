import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import BookList from './components/BookList';
import ReservationList from './components/ReservationList';
import Login from './components/Login';
import Registration from './components/Registration';
import PrivateRoute from './components/PrivateRoute';
import Home from './components/Home';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
    const user = JSON.parse(localStorage.getItem('user'));

    return (
        <div>
            <Routes>
                <Route path="/" element={user ? <Navigate to="/books" /> : <Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Registration />} />
                <Route
                    path="/books"
                    element={
                        <PrivateRoute>
                            <BookList />
                        </PrivateRoute>
                    }
                />
                <Route
                    path="/reservations"
                    element={
                        <PrivateRoute>
                            <ReservationList />
                        </PrivateRoute>
                    }
                />
                <Route path="*" element={<Navigate to="/" />} />
            </Routes>
        </div>
    );
}

export default App;
