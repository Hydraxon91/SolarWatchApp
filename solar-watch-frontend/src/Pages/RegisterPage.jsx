import RegisterElement from "../Components/RegisterElement";
import { Link } from 'react-router-dom';

export default function RegisterPage(){

    return(
           
        <div>
            <RegisterElement></RegisterElement>
            <Link to="/">
                <button>Frontpage</button>
            </Link>
        </div>
    
        )
    }