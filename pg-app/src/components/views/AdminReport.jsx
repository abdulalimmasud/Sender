import React, { Component } from "react";
import { View, Text, StyleSheet } from "react-native";
import Container from "../common/Container";
import Colors from "../utils/colors";
import ReportNavigator from "./navigator/ReportNavigator";
import CustomHeader from "../common/CustomHeader";

class AdminReport extends Component {
  static router = ReportNavigator.router;
  render() {
    return (
      <Container>
        <CustomHeader navigation={this.props.navigation} />
        <ReportNavigator navigation={this.props.navigation} />
        {/* <View style={styles.container}>
          
        </View> */}
      </Container>
    );
  }
}
const styles = StyleSheet.create({
  container: {
    backgroundColor: "black",
    alignItems: "center",
    justifyContent: "center",
    padding: 50
  }
});

export default AdminReport;
