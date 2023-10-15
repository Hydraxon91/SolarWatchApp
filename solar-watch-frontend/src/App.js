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

    const logout = () => {
        setUser(null);
        cookies.remove("jwt_authorization");
    }

    useEffect(() => {
        const token = cookies.get('jwt_authorization');
        
        if (!token) {
            setUser(null); 
            // window.location = '/';
        } else if (!user) {
            const decoded = decodeToken(token);
            setUser(decoded);
        }
    }, [cookies, user]);

    const decodeToken = (token) => {
        return jwt(token);
    }

    return (
        <BrowserRouter>
        <Layout user={user} newStopFrame={newStopFrame} setNewStopFrame={setNewStopFrame} logout={logout}>
            <AppRoutes user={user} setUser={setUser} cookies={cookies} setNewStopFrame={setNewStopFrame} logout={logout}/>
        </Layout>
        </BrowserRouter>
    );
}
