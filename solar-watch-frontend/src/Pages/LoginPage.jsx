import React, { useEffect, useRef } from 'react';
import LoginElement from "../Components/LoginElement";
import { Link, useNavigate } from 'react-router-dom';
import "../styles/login.css";

export default function LoginPage(props){

    const navigate = useNavigate();
    const containerRef = useRef();
    useEffect(() => {
        const handleClickOutside = (e) => {
          if (containerRef.current && !containerRef.current.contains(e.target)) {
            navigate('/'); // Navigate to the front page when clicking outside the container
          }
        };
    
        document.addEventListener('mousedown', handleClickOutside);
    
        return () => {
          document.removeEventListener('mousedown', handleClickOutside);
        };
      }, [navigate]);

    return(
            <div className="page-element-container"  ref={containerRef}>
                <LoginElement cookies={props.cookies} setUser={props.setUser} user={props.user}></LoginElement>
            </div>
        )
    }