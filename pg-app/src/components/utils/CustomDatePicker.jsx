import React, { useState } from "react";
import { Keyboard, TouchableWithoutFeedback } from "react-native";
import { Item, Input, Label, Form } from "native-base";
import moment from "moment";
import DateTimePicker from "react-native-modal-datetime-picker";
import Container from "../common/Container";

const CustomDatePicker = props => {
  const [show, setShow] = useState(false);
  const [value, setValue] = useState("");
  const [mode, setMode] = useState("date");
  const [format, setFormat] = useState("DD/MM/YYYY");
  const [label, setLabel] = useState("Date");

  const showDateTimePicker = () => {
    Keyboard.dismiss();
    setShow(true);
  };
  const hideDateTimePicker = () => {
    setShow(false);
  };
  const handleDatePicked = val => {
    setValue(val);
  };
  return (
    <Container>
      <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
        <Form onPress={showDateTimePicker}>
          <Item floatingLabel onKeyPress={showDateTimePicker}>
            <Input
              caretHidden
              value={value ? moment(value).format(format) : ""}
              onFocus={showDateTimePicker}
            />
            <Label>{label}</Label>
          </Item>
          <DateTimePicker
            date={value ? new Date(value) : new Date()}
            isVisible={show}
            mode={mode}
            onConfirm={handleDatePicked}
            onCancel={hideDateTimePicker}
          />
        </Form>
      </TouchableWithoutFeedback>
    </Container>
  );
};

export default CustomDatePicker;
