import React from "react";
import { Text, StyleSheet } from "react-native";
import Container from "../../common/Container";
import Colors from "../../utils/colors";

const CustomReport = props => {
  return (
    <Container style={styles.container}>
      <Text>Custom Report</Text>
    </Container>
  );
};
const styles = StyleSheet.create({
  container: {
    backgroundColor: Colors.green,
    alignItems: "center",
    justifyContent: "center",
    padding: 50
  }
});

export default CustomReport;
