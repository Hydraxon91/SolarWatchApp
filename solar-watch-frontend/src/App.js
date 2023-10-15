import React, { useState, useEffect } from 'react';
import { BrowserRouter } from 'react-router-dom';
import Cookies from 'universal-cookie';
import jwt from 'jwt-decode';
import Layout from './Components/Layout';
import AppRoutes from './AppRoutes';

const sunriseFrame = 100;

export default function App() {
    const cookies = new Cookies();
    const [user, setUser] = useState(null);
    const [newStopFrame, setNewStopFrame] = useState(sunriseFrame);
    const [expirationTimer, setExpirationTimer] = useState(null);
    let tokenExpirationTimer;

    const logout = () => {
        setUser(null);
        cookies.remove("jwt_authorization");
    }

    const checkTokenExpiration = () => {
        const token = cookies.get('jwt_authorization');
        if (!token) {
            setUser(null);
            clearTokenExpirationTimer(); // Clear the timer when the token expires
            
        } else if (!user) {
            const decoded = decodeToken(token);
            setUser(decoded);
        }
        return token;
    }

    const clearTokenExpirationTimer = () => {
        if (tokenExpirationTimer) {
            clearTimeout(tokenExpirationTimer); // Clear the timer
        }
    }

    const decodeToken = (token) => {
        return jwt(token);
    }

    useEffect(() => {
        const token = checkTokenExpiration();
        if (token && user) {
            const decoded = decodeToken(token);
            const expirationTime = decoded.exp * 1000;
            const timeRemaining = expirationTime - Date.now();
            if (timeRemaining > 0) {
                tokenExpirationTimer = setTimeout(checkTokenExpiration, timeRemaining);
                setExpirationTimer(timeRemaining);
            }    
        }
    }, [cookies, user]);

    
    return (
        <BrowserRouter>
        <Layout user={user} newStopFrame={newStopFrame} setNewStopFrame={setNewStopFrame} logout={logout}>
            <AppRoutes 
                user={user} setUser={setUser} cookies={cookies} 
                setNewStopFrame={setNewStopFrame} logout={logout}
                expirationTimer={expirationTimer}    
            />
        </Layout>
        </BrowserRouter>
    );
}
