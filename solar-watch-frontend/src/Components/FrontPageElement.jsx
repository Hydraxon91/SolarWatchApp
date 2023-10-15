import React, { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import "../styles/frontpage.css";

const targetFrame = 182;

export default function FrontPageElement({user, logout, expirationTimer}){

    const [remainingTime, setRemainingTime] = useState(expirationTimer);

    const formatTime = (time) => {
        const minutes = Math.floor(time / 60000);
        const seconds = ((time % 60000) / 1000).toFixed(0);
        return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
      };

      useEffect(() => {
        // Update the remaining time every second
        const interval = setInterval(() => {
          setRemainingTime((prevTime) => prevTime - 1000);
        }, 1000);
    
        // Clear the interval when the component unmounts
        return () => clearInterval(interval);
      }, []);

      useEffect(() => {
        setRemainingTime(expirationTimer);
      }, [expirationTimer]);

    return(
        <div className="curr-page">

            <div className="front-page-element-container">
                
                {
                    user? (
                        <div className='frontpage'>
                            <p>Welcome {user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]}!</p>
                            <p>Time remaining: {formatTime(remainingTime)}</p>
                            <button onClick={logout}>Logout</button>
                            <Link to="/solar-watch" className="button-link">
                                <button>SolarWatch</button>
                            </Link>
                            
                        </div>
                    ):
                    (
                    <div className='frontpage'>
                        <p>Welcome to SolarWatch!</p>
                        <Link to="/registration" className="button-link">
                            <button>Register</button>
                        </Link>
                        <Link to="/login" className="button-link">
                            <button>Login</button>
                        </Link>
                        
                    </div>
                    )
                }
            </div>
        </div>
        )
    }