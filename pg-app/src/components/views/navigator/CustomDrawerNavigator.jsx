import React from "react";
import {
  View,
  StyleSheet,
  SafeAreaView,
  ScrollView,
  Image
} from "react-native";
import { DrawerItems } from "react-navigation-drawer";

const CustomDrawerNavigator = props => (
  <SafeAreaView style={styles.container}>
    <View style={styles.imgContainer}>
      <Image source={require("../../../../assets/icon.png")} />
    </View>
    <ScrollView>
      <DrawerItems
        activeBackgroundColor={"#000"}
        activeTintColor={"#fff"}
        iconContainerStyle={styles.icons}
        {...props}
      />
    </ScrollView>
  </SafeAreaView>
);
const styles = StyleSheet.create({
  container: {
    flex: 1,
    paddingTop: 40
  },
  imgContainer: {
    height: 60,
    alignItems: "center",
    justifyContent: "center"
  },
  icons: {
    width: 30
  }
});

export default CustomDrawerNavigator;
