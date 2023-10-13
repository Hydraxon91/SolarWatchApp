import React, { useState, useEffect, useRef } from 'react';
import { BrowserRouter } from 'react-router-dom';
import Cookies from 'universal-cookie';
import jwt from 'jwt-decode';
import Layout from './Components/Layout';
import AppRoutes from './AppRoutes';

const sunriseFrame = 100;
const middayFrame = 162;
const nightFrame = 364;
const sunsetFrame = 50;


function App() {
    const cookies = new Cookies();
    const [user, setUser] = useState(null);
    const [newStopFrame, setNewStopFrame] = useState(middayFrame);
    useEffect(() => {
        const token = cookies.get('jwt_authorization');
        if (token && !user) {
        // Decode and set the user here based on the token
        const decoded = decodeToken(token);
        setUser(decoded);
        }
    }, [cookies, user]);

    const decodeToken = (token) => {
        return jwt(token);
    }

    return (
        <BrowserRouter>
        <Layout user={user} newStopFrame={newStopFrame} setNewStopFrame={setNewStopFrame}>
            <AppRoutes user={user} setUser={setUser} cookies={cookies} setNewStopFrame={setNewStopFrame} />
        </Layout>
        </BrowserRouter>
    );
}

export default App;