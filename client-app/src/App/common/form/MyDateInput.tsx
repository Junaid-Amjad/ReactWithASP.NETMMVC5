import { useField } from "formik"
import { Form, Label, Select } from "semantic-ui-react"
import DatePicker,{ReactDatePickerProps} from 'react-datepicker'


export default function MyDateInput(props:Partial<ReactDatePickerProps>){
    const [fields,meta,helpers] = useField(props.name!)
    return(
        <Form.Field error={meta.touched && !!meta.error}>
            <DatePicker 
                {...fields}
                {...props}
                selected={(fields.value && new Date(fields.value)) || null}
                onChange={value => helpers.setValue(value)}
            />
            {meta.touched && meta.error ? (
                <Label basic color='red'>{meta.error}</Label>
            ) : null}
        </Form.Field>

    )
}
