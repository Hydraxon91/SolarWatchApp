import React from 'react';
import { Link } from 'react-router-dom';

export default function FrontPage({cookies, user, setUser}){

    const logout = () => {
        setUser(null);
        cookies.remove("jwt_authorization");
    }

    return(
        <>
           {
            user? (
                <div>
                    <p>Welcome {user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]}!</p>
                    <button onClick={logout}>Logout</button>
                    <Link to="/solar-watch">
                        <button>SolarWatch</button>
                    </Link>
                </div>
            ):
            (
            <div>
                <h1>Welcome to the Front Page</h1>
                <Link to="/registration">
                    <button>Register</button>
                </Link>
                <Link to="/login">
                    <button>Login</button>
                </Link>
                
            </div>
            )
           }
        </>
        )
    }