import React, { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import "../styles/lotty.css";

const targetFrame = 182;

export default function FrontPage({cookies, user, setUser, logout}){
    return(
        <div className="curr-page">

            <div className="page-element-container">
                
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
            </div>
        </div>
        )
    }