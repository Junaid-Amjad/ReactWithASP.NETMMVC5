import { Message } from "semantic-ui-react";

interface Props{
    errors:string[] | null;
}

export default function ValidationError({errors}: Props){
    return(
        <Message error>
            {errors && (
                <Message.List>
                    {console.log(errors)}
                    {errors.map((err:any,i)=>(
                        <Message.Item key={i}>{err}</Message.Item>
                    ))}
                </Message.List>
            )}
        </Message>
    )
}