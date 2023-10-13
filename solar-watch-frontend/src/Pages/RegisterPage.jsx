import React, { useEffect, useRef } from 'react';
import RegisterElement from "../Components/RegisterElement";
import { Link, useNavigate } from 'react-router-dom';
import "../styles/lotty.css";

export default function RegisterPage(){

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
            <div className="register-page-element-container" ref={containerRef}>
                <RegisterElement></RegisterElement>
            </div>
    
        )
    }