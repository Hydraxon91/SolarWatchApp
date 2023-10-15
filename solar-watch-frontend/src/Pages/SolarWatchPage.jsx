import { Link, useNavigate } from 'react-router-dom';
import React, { useState, useEffect, useRef } from 'react';
import SolarInfoElement from '../Components/SolarInfoElement';
import SolarWatchElement from '../Components/SolarWatchElement';
import "../styles/lotty.css";
import "../styles/solarwatchpage.css";

export default function SolarWatchPage({cookies, setNewStopFrame}){


    return(
            <SolarWatchElement cookies={cookies} setNewStopFrame={setNewStopFrame}></SolarWatchElement>
        )
}