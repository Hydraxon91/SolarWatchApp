import React, { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import FrontPageElement from '../Components/FrontPageElement';
import "../styles/lotty.css";

const targetFrame = 182;

export default function FrontPage({ user, logout, expirationTimer}){
    return(
        <FrontPageElement user={user} logout={logout} expirationTimer={expirationTimer}></FrontPageElement>
        )
    }