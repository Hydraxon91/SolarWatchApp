import LoginElement from "../Components/LoginElement";
import { Link } from 'react-router-dom';

export default function LoginPage(props){

    return(
           
        <div>
            <LoginElement cookies={props.cookies} setUser={props.setUser} user={props.user}></LoginElement>
            <Link to="/">
                <button>Frontpage</button>
            </Link>
        </div>
    
        )
    }