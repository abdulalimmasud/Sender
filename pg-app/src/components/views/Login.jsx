import React from "react";
import {
  Keyboard,
  Text,
  TextInput,
  TouchableWithoutFeedback,
  Alert,
  KeyboardAvoidingView,
  Button,
  StyleSheet
} from "react-native";
import Container from "../common/Container";
import Colors from "../utils/colors";

const Login = props => {
  const onLoginHandler = () => {
    return props.navigation.navigate("UserPanel");
  };
  return (
    <KeyboardAvoidingView style={styles.container}>
      <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
        <Container>
          <Text style={styles.logoText}>Xeekar</Text>
          <TextInput
            style={styles.textInput}
            placeholder="Username"
            placeholderTextColor={Colors.frenchGray}
          />
          <TextInput
            style={styles.textInput}
            placeholder="Password"
            placeholderTextColor={Colors.frenchGray}
            secureTextEntry={true}
          />
          <Button
            style={styles.button}
            title="Login"
            onPress={onLoginHandler}
          />
        </Container>
      </TouchableWithoutFeedback>
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: { flex: 1 },
  logoText: {
    fontSize: 40,
    fontWeight: "800",
    marginTop: 150,
    marginBottom: 30,
    textAlign: "center"
  },
  textInput: {
    height: 43,
    fontSize: 14,
    borderRadius: 5,
    borderWidth: 1,
    borderColor: Colors.alabasterSolid,
    backgroundColor: Colors.galleryApprox,
    paddingLeft: 10,
    marginLeft: 15,
    marginRight: 15,
    marginTop: 5,
    marginBottom: 5
  },
  button: {
    backgroundColor: Colors.pictonBlue,
    borderRadius: 5,
    paddingLeft: 10,
    marginLeft: 15,
    marginRight: 15,
    marginTop: 10
  }
});

export default Login;
