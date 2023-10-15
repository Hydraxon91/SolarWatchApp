import { useState } from 'react'
import { NavLink } from 'react-router-dom'
import { ReactComponent as Brand } from '../Assets/icons/logo.svg'
import '../styles/navbar.css';

export default function Navbar({logout, user}){

    
    return (
      <nav className='navbar'>
        <div className='container'>
            <Brand className='logo'/>
            <div className={`nav-elements`}>
            <ul>
                <li><NavLink to="/">Home</NavLink></li>
                {
                    user ? (
                        <>
                            <li><NavLink to="/solar-watch">Solarwatch</NavLink></li>
                            <li className='username'>user: {user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]}</li>
                            <li onClick={logout} className='logout'>Logout</li>
                        </>
                    ) : (
                        <>
                            <li><NavLink to="/registration">Register</NavLink></li>
                            <li><NavLink to="/login">Login</NavLink></li>
                        </>
                    )
                }
            </ul>
            </div>
        </div>
      </nav>
    )
  }