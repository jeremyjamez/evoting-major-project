import { Button, Grid, Input, Spacer, Text, useCurrentState } from "@geist-ui/react";
import { useForm } from 'react-hook-form'
import styles from '../styles/layout.module.css'
import { yupResolver } from '@hookform/resolvers/yup'
import * as yup from 'yup'
import { useEffect, useState } from "react";

const schema = yup.object().shape({
    answer: yup.string().required('This is a required field.')
})

const normalizeInput = (value, previousValue) => {
    if (!value) return value;
    const currentValue = value.replace(/[^\d]/g, '');
    const cvLength = currentValue.length;

    if (!previousValue || value.length > previousValue.length) {
        if (cvLength < 4) return currentValue;
        if (cvLength < 7) return `${currentValue.slice(0, 3)}-${currentValue.slice(3)}`;
        return `${currentValue.slice(0, 3)}-${currentValue.slice(3, 6)}-${currentValue.slice(6, 10)}`;
    }
};

const SecurityQuestion = ({ item, triggerPushAnswer, triggerPushAttempt, next, number }) => {

    const [attempt, setAttempt, attemptRef] = useCurrentState(3)
    const [answer, setAnswer] = useState('')

    const { register, setValue, errors, handleSubmit, setError } = useForm({
        resolver: yupResolver(schema),
        criteriaMode: 'all',
        shouldFocusError: true
    })

    const handleAnswer = (e) => {
        if(item.Question === 'Telephone number'){
            setAnswer(normalizeInput(e.target.value, answer))
            setValue('answer', normalizeInput(e.target.value, answer))
        } else {
            setAnswer(e.target.value)
            setValue('answer', e.target.value)
        }
    }

    useEffect(() => {
        register('answer', {required: true})
    },[register])

    const onSubmit = (data) => {
        setAttempt((prev) => prev - 1)
        triggerPushAttempt(attemptRef.current)
        if (data.answer.toLowerCase() === item.Answer.toLowerCase()) {
            triggerPushAnswer()
            next()
        } else {
            setError("answer", { type: 'manual', message: 'Your answer is incorrect. Please check the spelling or format of your response and try again.' })
        }
    }

    return (
        <>
            <Grid.Container gap={2} justify="center">
                <Grid xs={24}>
                    <Text style={{ textAlign: 'center' }} h1>What is your {item.Question}?</Text>
                </Grid>
                <Grid xs={24}>
                    <form onSubmit={handleSubmit(onSubmit)} style={{width: '100%'}}>
                        <Grid.Container gap={1}>
                            <Grid xs={24}>
                                <Input style={{ fontSize: '1.5rem' }} onChange={handleAnswer} value={answer} name="answer" placeholder="Type answer here" size="large" width="100%" status="secondary" clearable />
                            </Grid>
                            <Grid xs={24}>
                                {
                                    errors.answer && <Text className={styles.formError} span>{errors.answer?.message}</Text>
                                }
                            </Grid>
                            <Grid xs={24} justify="flex-end">
                                <Button htmlType="submit" type="secondary" size="large" auto shadow>Next</Button>
                            </Grid>
                        </Grid.Container>
                    </form>
                </Grid>
            </Grid.Container>
        </>
    )
}

export default SecurityQuestion